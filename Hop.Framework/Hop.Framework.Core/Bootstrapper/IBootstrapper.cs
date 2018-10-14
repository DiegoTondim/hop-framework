using Hop.Framework.Core.IoC;

namespace Hop.Framework.Core.Bootstrapper
{
    public interface IBootstrapper
    {
        IBootstrapperLifestyle UseDI<T>() where T : IContainer;
        IBootstrapperLifestyle UseDI<T>(T container) where T : IContainer;
    }

    public interface IBootstrapperLifestyle
    {
        IBootstrapperModules ThreadLifestyle();
        IBootstrapperWebLifestyle WebLifestyle();
    }

    public interface IBootstrapperWebLifestyle : IBootstrapperModules
    {
    }

    public enum Lifestyle
    {
        Thread,
        Web
    }
}
