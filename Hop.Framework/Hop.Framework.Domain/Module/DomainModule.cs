using Hop.Framework.Core.IoC;
using Hop.Framework.Domain.Notification;

namespace Hop.Framework.Domain.Module
{
    public class DomainModule : DependencyModule
    {
        public override void Load(IContainer container)
        {
            container.Register<IDomainNotificationHandler, DomainNotificationHandler>(Lifestyle.Scoped);
        }
    }
}
