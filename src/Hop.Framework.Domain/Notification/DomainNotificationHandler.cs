using System.Collections.Generic;
using System.Linq;

namespace Hop.Framework.Domain.Notification
{
    public class DomainNotificationHandler : IDomainNotificationHandler
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public void Handle(DomainNotification message)
        {
            _notifications.Add(message);
        }

        public virtual IEnumerable<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        public virtual bool HasNotificationsWithKey(string key)
        {
            return GetNotifications().Any(x => x.Key == key);
        }

        public void Dispose()
        {
            _notifications = new List<DomainNotification>();
        }
    }
}
