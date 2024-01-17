using CQRS;
using EventStore.Services;
using EventStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Orders.IntegrationEvents;
using System.Reflection;

namespace OrderService.Application
{
    public static class ServiceCollectionExtention
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
        {
            services.AddCqrs(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection AddIntegrationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("EventStoreConnectionString"));
            });

            services.AddTransient<IIntegrationEventLogService>(sp =>
            {
                var eventLogContext = sp.GetRequiredService<IntegrationEventLogContext>();
                return new IntegrationEventLogService(eventLogContext, Assembly.GetExecutingAssembly());
            });

            services.AddTransient<IOrderIntegrationEventService, OrderIntegrationEventService>();

            return services;
        }
    }
}
