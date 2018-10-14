using System;
using System.Security.Claims;
using Hop.Framework.Core.IoC;
using Hop.Framework.Core.Tests.Bootstrapper;
using Hop.Framework.Core.User;
using NUnit.Framework;

namespace Hop.Framework.Core.Tests.Context
{
    [TestFixture]
    public class ContextServiceTest
    {
        public ContextServiceTest()
        {
            Core.Bootstrapper.Bootstrapper
                .Configure()
                .UseDI<ContainerTest>()
                .ThreadLifestyle()
                .RegisterModule<InfrastructureModule>()
                .Build();
        }
        [Test]
        public void Should_Set_Context_In_Service()
        {
            var service = ServiceResolver.Container.Resolve<IUserContextService>();
            service.Set(new ContextTest(Guid.NewGuid(), "John", Guid.NewGuid()));

            Assert.IsNotNull(service.Get<ContextTest>());
            Assert.IsNotNull(service.Get<ContextTest>().TenantId);
            Assert.IsNotNull(service.UserContext);
            Assert.AreEqual("John", service.UserContext.Name);
        }
    }

    public class InfrastructureModule : DependencyModule
    {
        public override void Load(IContainer container)
        {
            container.Register<IUserContextService, UserContextService>();
        }
    }

    public class ContextTest : UserContextBase
    {
        public Guid TenantId { get; set; }
        public ContextTest(Guid id, string name, Guid tenantId) : base(id, name, name, new Claim[0])
        {
            TenantId = tenantId;
        }
    }
}
