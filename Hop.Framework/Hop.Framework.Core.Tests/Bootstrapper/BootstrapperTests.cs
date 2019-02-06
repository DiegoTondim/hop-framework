using Hop.Framework.Core.IoC;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Hop.Framework.Core.Tests.Bootstrapper
{
    [TestFixture]
    public class BootstrapperTests
    {
        private readonly IContainer _container;

        public BootstrapperTests()
        {
            _container = new ContainerTest();
        }

        [Test]
        public void Should_Create_Dependency_Module_And_Resolve_A_Service()
        {
            new DependencyModuleTest().Load(_container);
            var service = _container.Resolve<IServiceTest>();
            Assert.IsNotNull(service);
        }

        [Test]
        public void Should_Start_IoC_And_Register_Module_Resolving_With_Service_Resolver()
        {
            Core.Bootstrapper.Bootstrapper
                .Configure()
                .UseDI<ContainerTest>()
                .WebLifestyle()
                .RegisterModule<DependencyModuleTest>()
                .Build();
            var service = ServiceResolver.Container.Resolve<IServiceTest>();
            Assert.IsNotNull(service);
        }

        [Test]
        public void Should_Start_IoC_As_Web_API_Lifestyle()
        {
            Core.Bootstrapper.Bootstrapper
                .Configure()
                .UseDI<ContainerTest>()
                .WebLifestyle()
                .RegisterModule<DependencyModuleTest>()
                .Build();
            var service = ServiceResolver.Container.Resolve<IServiceTest>();
            Assert.IsNotNull(service);
        }

        [Test]
        public void Should_Start_IoC_As_Thread_Lifestyle()
        {
            Core.Bootstrapper.Bootstrapper
                .Configure()
                .UseDI<ContainerTest>()
                .ThreadLifestyle()
                .RegisterModule<DependencyModuleTest>()
                .Build();
            var service = ServiceResolver.Container.Resolve<IServiceTest>();
            Assert.IsNotNull(service);
        }
    }

    public interface IServiceTest
    {
    }

    public class ServiceTest : IServiceTest
    {
    }

    public class DependencyModuleTest : DependencyModule
    {
        public override void Load(IContainer container)
        {
            container.Register<IServiceTest, ServiceTest>();
        }
    }

    public class ContainerTest : IContainer
    {
        public readonly IDictionary<Type, Type> _types;

        public ContainerTest()
        {
            _types = new Dictionary<Type, Type>();
        }

        public void Dispose()
        {
        }

        public void UseWebApiLifestyle()
        {
        }

        public void UseThreadLifestyle()
        {
        }

        public void Register(Type @from, Type to, Lifestyle lifestyle)
        {
        }

        public void Register(Type @from, Func<IContainer, object> to, Lifestyle lifestyle)
        {
        }

        public void Register<TFrom, TTo>(Lifestyle lifestyle = Lifestyle.Transient) where TTo : TFrom
        {
            _types.Add(typeof(TFrom), typeof(TTo));
        }

        public void Register<TFrom>(Func<IContainer, object> to, Lifestyle lifestyle = Lifestyle.Transient)
        {
        }

        public T Resolve<T>()
        {
            return (T)Activator.CreateInstance(_types[typeof(T)]);
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
            return null;
        }

        public object ResolveOrDefault(Type t)
        {
            return null;
        }

        public IContainerScoped BeginScope(ScopeType type)
        {
            return null;
        }

        public IContainer CreateChildContainer()
        {
            return null;
        }

		public void Load(DependencyModule module)
		{
		}
	}
}
