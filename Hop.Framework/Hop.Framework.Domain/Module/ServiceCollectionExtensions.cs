using Hop.Framework.Domain.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Framework.Domain.Module
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomain(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IDomainNotificationHandler, DomainNotificationHandler>();
        }
    }
}
