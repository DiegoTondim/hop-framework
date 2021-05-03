using Hop.Framework.Core.User;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Framework.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUserContextService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUserContextService, UserContextService>();
            return serviceCollection;
        }
    }
}
