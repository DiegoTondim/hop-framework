using System;
using Hop.Framework.Core.IoC;

namespace Hop.Framework.Core.Bootstrapper
{
    public class Bootstrapper : IBootstrapper, IBootstrapperModulesLoader, IBootstrapperModules, IBootstrapperLifestyle, IBootstrapperWebLifestyle
    {
        private Lifestyle _lifestyle;
        protected readonly IContainer _container;
        //private HttpConfiguration _httpConfiguration;
        private bool _updateDatabase = false;
        private Action<IContainer> _registerServicesByAction;
        private static Bootstrapper _instance;

        private Bootstrapper()
        {
        }

        public Bootstrapper(IContainer container)
        {
            _container = container;
        }

        public Bootstrapper UpdateDatabaseAfterStart()
        {
            _updateDatabase = true;
            return this;
        }

        public IBootstrapperModules RegisterModule(Action<IBootstrapperModulesLoader> loader)
        {
            loader(this);
            return this;
        }

        IBootstrapperModules IBootstrapperModules.RegisterModule<T>()
        {
            this.RegisterModule<T>();
            return this;
        }

        public void Build()
        {
            if (_container == null)
                throw new Exception("IoC Provider not found.");

            if (_updateDatabase)
            {
                //#if !DEBUG
                //                var updater = _container.Resolve<IDatabaseUpdaterBootstrapper>();
                //                updater.Update();
                //#endif
            }
        }

        public static IBootstrapper Configure() => new Bootstrapper();

        public IBootstrapperLifestyle UseDI<T>() where T : IContainer
        {
            _instance = new Bootstrapper(Activator.CreateInstance<T>());
            ServiceResolver.Register(_instance._container);
            return _instance;
        }

        public IBootstrapperLifestyle UseDI<T>(T container) where T : IContainer
        {
            _instance = new Bootstrapper(container);
            ServiceResolver.Register(_instance._container);
            return _instance;
        }

        public IBootstrapperModulesLoader RegisterModule<T>() where T : IModule
        {
            var module = Activator.CreateInstance<T>();
            module.Load(_container);
            return this;
        }

        public IBootstrapperModules ThreadLifestyle()
        {
            _container.UseThreadLifestyle();
            _lifestyle = Lifestyle.Thread;
            return this;
        }

        public IBootstrapperWebLifestyle WebLifestyle()
        {
            _lifestyle = Lifestyle.Web;
            return this;
        }

        public IContainer Container() => _container;
    }
}
