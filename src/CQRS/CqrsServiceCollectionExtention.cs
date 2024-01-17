using CQRS.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CQRS
{
    public static class CqrsServiceCollectionExtention
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(assembly);
            services.AddScoped<ICommandBus, CommandBus>();

            return services;
        }
    }
}
