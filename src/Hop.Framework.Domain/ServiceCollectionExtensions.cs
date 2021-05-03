using Hop.Framework.Domain.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Framework.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainNotifications(this IServiceCollection services) =>
            services.AddScoped<IDomainNotificationHandler, DomainNotificationHandler>();
    }
}
