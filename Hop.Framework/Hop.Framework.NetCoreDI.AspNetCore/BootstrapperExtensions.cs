using Hop.Framework.Core.Bootstrapper;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Framework.NetCoreDI.AspNetCore
{
    public static class BootstrapperExtensions
    {
        public static IBootstrapperModules UseNetCoreDI(this IBootstrapper bootstrapper, IServiceCollection serviceCollection)
        {
            return bootstrapper
                .UseDI(new NetCoreDIContainer(serviceCollection))
                .WebLifestyle();
        }
    }
}
