using System;
using System.Collections.Generic;

namespace Hop.Framework.Domain.Notification
{
    public interface IDomainNotificationHandler : IDisposable
    {
        bool HasNotificationsWithKey(string key);
        void Handle(DomainNotification message);
        IEnumerable<DomainNotification> GetNotifications();
        bool HasNotifications();
    }
}
