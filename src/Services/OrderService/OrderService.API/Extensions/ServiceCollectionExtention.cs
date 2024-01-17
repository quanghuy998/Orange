using RabbitMQ.Client;
using EventBus;
using EventBus.RabbitMQ;
using MediatR;
using CQRS.Behaviors;
using OrderService.Behaviors;
using OrderService.Application.Orders.IntegrationEvents.Events;
using OrderService.Application.Orders.IntegrationEvents.EventHandlers;
using System.Data.Common;
using EventStore.Services;
using EventStore;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Orders.IntegrationEvents;
using System.Reflection;

namespace OrderService.Extensions
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddEventBusEventHandlers(this IServiceCollection collection)
        {
            collection.AddTransient<OrderStartedIntegrationEventHandler>();
            return collection;
        }

        public static IServiceProvider AddEventBusSubcribes(this IServiceProvider provider)
        {
            var eventBus = provider.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
            return provider;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var eventBusSection = configuration.GetSection("EventBus");
            if (!eventBusSection.Exists())
                return services;

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddSingleton<IRabbitMQPersistentConnection>(provider =>
            {
                var connectionFactory = new ConnectionFactory()
                {
                    HostName = configuration.GetConnectionString("EventBus"),
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(eventBusSection["UserName"]))
                    connectionFactory.UserName = eventBusSection["UserName"];

                if (!string.IsNullOrEmpty(eventBusSection["Password"]))
                    connectionFactory.Password = eventBusSection["Password"];

                var logger = provider.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                return new DefaultRabbitMQPersistentConnection(connectionFactory, logger);
            });

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(serviceProvider =>
            {
                var retryCount = eventBusSection.GetValue("RetryCount", 5);
                var logger = serviceProvider.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var subscriptionClientName = eventBusSection.GetValue<string>("SubscriptionClientName");
                var rabbitMQPersistentConnection = serviceProvider.GetRequiredService<IRabbitMQPersistentConnection>();
                var eventBusSubscriptionsManager = serviceProvider.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMQPersistentConnection,
                                            eventBusSubscriptionsManager,
                                            serviceProvider,
                                            logger,
                                            subscriptionClientName,
                                            retryCount);
            });

            return services;
        }

        public static IServiceCollection AddBehaviors(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }
    }
}
