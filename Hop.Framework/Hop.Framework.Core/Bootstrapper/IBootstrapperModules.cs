using System;
using Hop.Framework.Core.IoC;

namespace Hop.Framework.Core.Bootstrapper
{
    public interface IBootstrapperModules
    {
        IBootstrapperModules RegisterModule(Action<IBootstrapperModulesLoader> loader);
        IBootstrapperModules RegisterModule<T>() where T : IModule;
        void Build();
    }

    public interface IBootstrapperModulesLoader
    {
        IBootstrapperModulesLoader RegisterModule<T>() where T : IModule;
    }
}