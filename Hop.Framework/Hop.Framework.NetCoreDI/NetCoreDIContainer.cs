using System;
using System.Collections.Generic;
using Hop.Framework.Core.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Hop.Framework.NetCoreDI
{
    public class NetCoreDIContainer : IContainer
    {
        protected readonly IServiceCollection _register;
        protected IServiceProvider _resolver;
        protected IServiceProvider Resolver => (_resolver ?? (_resolver = _register.BuildServiceProvider()));
        private IServiceScope _scope;

        public NetCoreDIContainer(IServiceCollection register)
        {
            _register = register;
        }

        public NetCoreDIContainer(IServiceProvider resolver)
        {
            _resolver = resolver;
        }

        public NetCoreDIContainer(IServiceCollection register, IServiceScope scope)
        {
            _resolver = scope.ServiceProvider;
            _scope = scope;
        }

        public void UseThreadLifestyle()
        {
        }

        public void Register<TFrom, TTo>(Core.IoC.Lifestyle lifestyle = Core.IoC.Lifestyle.Transient) where TTo : TFrom
        {
            Register(typeof(TFrom), typeof(TTo), lifestyle);
        }

        public void Register(Type from, Type to, Lifestyle lifestyle)
        {
            switch (lifestyle)
            {
                case Lifestyle.Singleton:
                    _register.AddSingleton(from, to);
                    break;
                case Lifestyle.Transient:
                    _register.AddTransient(from, to);
                    break;
                default:
                    _register.AddScoped(from, to);
                    break;
            }
        }

        public void Register(Type from, Func<IContainer, object> to, Lifestyle lifestyle)
        {
            switch (lifestyle)
            {
                case Lifestyle.Singleton:
                    _register.AddSingleton(from, to(this));
                    break;
                case Lifestyle.Transient:
                    _register.AddTransient(from, provider => to(new NetCoreDIContainer(provider)));
                    break;
                default:
                    _register.AddScoped(from, provider => to(new NetCoreDIContainer(provider)));
                    break;
            }
        }

        public T Resolve<T>()
        {
            var fun = Resolver.GetRequiredService(typeof(T));
            if (fun.GetType() == typeof(Func<IContainer, Object>))
                return (T)(fun as Func<IContainer, object>)(this);
            return (T)fun;
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

        public object Resolve(Type t)
        {
            return Resolver.GetRequiredService(t);
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
            return new NetCoreDIScoped(_scope);
        }

        public IContainer CreateChildContainer()
        {
            var scope = _register.BuildServiceProvider().CreateScope();
            var container = new NetCoreDIContainer(null, scope);
            return container;
        }

        public void Register<TFrom>(Func<IContainer, object> to, Core.IoC.Lifestyle lifestyle = Core.IoC.Lifestyle.Transient)
        {
            Register(typeof(TFrom), to, lifestyle);
        }



        public object GetService(Type serviceType)
        {
            return Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Resolver.GetServices(serviceType);
        }

        public IServiceCollection InternalContainer()
        {
            return _register;
        }

        public void Dispose()
        {
        }
    }

    public class NetCoreDIScoped : IContainerScoped
    {
        private IServiceScope _scope;

        public NetCoreDIScoped(IServiceScope scope)
        {
            _scope = scope;
        }

        public void Dispose()
        {
            _scope.Dispose();
        }
    }
}
