using Hop.Framework.Domain.Dispatcher;
using Hop.Framework.Domain.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Framework.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainNotifications(this IServiceCollection services) =>
            services.AddScoped<IDomainNotificationHandler, DomainNotificationHandler>();

        public static IServiceCollection AddDispatcher(this IServiceCollection services) =>
            services.AddScoped<IDispatcher, Dispatcher.Dispatcher>();
    }
}
