using Hop.Framework.Core.IoC;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;

namespace Hop.Framework.UnitTests.DI
{
    public class UnitTestsContainer : IContainer
    {
        protected readonly Container _container;
        private IDictionary<Type, Func<IContainer, object>> _delegates;

        public UnitTestsContainer()
        {
            _delegates = new Dictionary<Type, Func<IContainer, object>>();
            _container = new Container();
            _container.Options.AllowOverridingRegistrations = true;
            _container.Options.SuppressLifestyleMismatchVerification = true;
        }

        public void UseThreadLifestyle()
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
        }

        public void Register(Type from, Func<IContainer, object> to, Core.IoC.Lifestyle lifestyle)
        {
            _delegates.Add(from, to);
            _container.Register(from, () => to(this), Converter(lifestyle));
        }

        public void Register<TFrom, TTo>(Core.IoC.Lifestyle lifestyle = Core.IoC.Lifestyle.Transient) where TTo : TFrom
        {
            Register(typeof(TFrom), typeof(TTo), lifestyle);
        }

        public void Register(Type from, Type to, Core.IoC.Lifestyle lifestyle)
        {
            _container.Register(from, to, Converter(lifestyle));
        }

        public T Resolve<T>()
        {
            return (T)_container.GetInstance(typeof(T));
        }

        public T ResolveOrDefault<T>()
        {
            try
            {
                return this.Resolve<T>();
            }
            catch
            {
                return default(T);
            }
        }

        private global::SimpleInjector.Lifestyle Converter(Core.IoC.Lifestyle lifestyle)
        {
            switch (lifestyle)
            {
                case Core.IoC.Lifestyle.Singleton:
                    return global::SimpleInjector.Lifestyle.Singleton;
                case Core.IoC.Lifestyle.Transient:
                    return global::SimpleInjector.Lifestyle.Transient;
                default:
                    return global::SimpleInjector.Lifestyle.Scoped;
            }
        }

        public object Resolve(Type t)
        {
            return _container.GetInstance(t);
        }

        public object ResolveOrDefault(Type t)
        {
            try
            {
                return this.Resolve(t);
            }
            catch
            {
                return null;
            }
        }

        public IContainerScoped BeginScope(ScopeType type)
        {
            return new UnitTestsContainerThreadScope(this._container);
        }

        public IContainer CreateChildContainer()
        {
            var container = new UnitTestsContainer();
            container.UseThreadLifestyle();
            foreach (var item in _container.GetCurrentRegistrations())
            {
                var lifestyle = item.Registration.Lifestyle;
                var internalLifestyle = Core.IoC.Lifestyle.Scoped;

                if (lifestyle == global::SimpleInjector.Lifestyle.Singleton)
                    internalLifestyle = Core.IoC.Lifestyle.Singleton;
                if (lifestyle == global::SimpleInjector.Lifestyle.Transient)
                    internalLifestyle = Core.IoC.Lifestyle.Transient;
                if (_delegates.ContainsKey(item.ServiceType))
                {
                    //container.InternalContainer().Register(item.ServiceType, () => item.GetInstance());
                    container.InternalContainer().Register(item.ServiceType, () => _delegates[item.ServiceType](container));
                }
                else if (item.Registration.ImplementationType.IsClass)
                    container.Register(item.ServiceType, item.Registration.ImplementationType, internalLifestyle);
            }

            return container;
        }

        public void Register<TFrom>(Func<IContainer, object> to, Core.IoC.Lifestyle lifestyle = Core.IoC.Lifestyle.Transient)
        {
            _delegates.Add(typeof(TFrom), to);
            _container.Register(typeof(TFrom), () => to(this), Converter(lifestyle));
        }

        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        public Container InternalContainer()
        {
            return _container;
        }

        public void Dispose()
        {
            _container.Dispose();
        }

		public void Load(DependencyModule module)
		{
		}
	}

    public class UnitTestsContainerThreadScope : IContainerScoped
    {
        private Scope _scope;

        public UnitTestsContainerThreadScope(Container container)
        {
            _scope = AsyncScopedLifestyle.BeginScope(container);
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
