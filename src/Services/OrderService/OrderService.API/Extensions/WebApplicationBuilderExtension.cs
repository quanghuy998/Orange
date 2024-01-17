using EventBus;
using EventBus.RabbitMQ;
using RabbitMQ.Client;

namespace OrderService.Extensions
{
    public static class WebApplicationBuilderExtension
    {
        public static WebApplicationBuilder AddMicroserviceRegistration(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddEventBus(configuration);
            return builder;
        }
    }
}
