using Hop.Framework.Core.Bootstrapper;

namespace Hop.Framework.UnitTests.DI
{
    public static class BootstrapperExtensions
    {
        public static IBootstrapperLifestyle UserUnitTestsContainer(this IBootstrapper bootstrapper)
        {
            return bootstrapper.UseDI<UnitTestsContainer>();
        }
    }
}
