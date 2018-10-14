using System;
using Hop.Framework.Core.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Framework.NetCoreDI.AspNetCore
{
    public static class ContainerExtensions
    {
        public static void RegisterDbContext<TFrom, TTo>(this IContainer container, string connectionName) where TTo : DbContext, TFrom where TFrom : class
        {
            var internalContainer = container as NetCoreDIContainer;
            var services = internalContainer
                .InternalContainer();

            services.AddDbContext<TTo>(options => options.UseSqlServer(connectionName));
            services.AddScoped<TFrom>(provider => provider.GetService<TTo>());
        }
        public static void RegisterDbContext<TFrom, TTo>(this IContainer container, string connectionName, Func<IContainer, object> function) where TTo : DbContext, TFrom where TFrom : class
        {
            var internalContainer = container as NetCoreDIContainer;
            var services = internalContainer
                .InternalContainer();

            services.AddDbContext<TTo>(options => options.UseSqlServer(connectionName));
            services.AddScoped<TFrom>((p) =>
            {
                return (TFrom)function(new NetCoreDIContainer(p));
            });
        }
    }
}
