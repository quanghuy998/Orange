using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OrderService.Domain.Buyers.Interfaces;
using OrderService.Domain.Orders.Interfaces;
using OrderService.Infrastructure.Buyers;
using OrderService.Infrastructure.Orders;

namespace OrderService.Infrastructure
{
    public static class ServiceCollectionExtention
    {
        public const string DEFAULT_SCHEMA = "OrderService";
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDatabase(configuration);
            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("OrderServiceConnectionString"));
            });

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IBuyerRepository, BuyerRepository>();

            return services;
        }
    }
}
