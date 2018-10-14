using Hop.Framework.Core.Bootstrapper;

namespace Hop.Framework.Domain.Module
{
    public static class BootstrapperExtensions
    {
        public static IBootstrapperModules UseDomainNotifications(this IBootstrapperModules bootstrapperModules)
        {
            bootstrapperModules.RegisterModule<DomainModule>();
            return bootstrapperModules;
        }
    }
}
