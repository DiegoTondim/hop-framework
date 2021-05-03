using System;
using System.Collections.Generic;

namespace Hop.Framework.Core.Messaging.Configuration
{
    public interface IMessagingConfiguration
    {
        IIntegrationModule GetModule();
        IMessagingConfigurationListener Subscribe<TModule>() where TModule : IIntegrationModule, new();
        IDictionary<IIntegrationModule, IMessagingConfigurationListener> GetSubscriptions();
        ListenerConfiguration GetListenerConfigurationFromMessage(Type messageType);
        int GetConsumersCount();
        bool ClearAllQueues();
    }
}
