using Hop.Framework.Domain.Repository;
using Hop.Framework.EFCore.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Framework.EFCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            return serviceCollection;
        }
    }
}
