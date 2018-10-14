using System;
using System.Collections.Generic;
using System.Linq;

namespace Hop.Framework.Core.Messaging.Configuration
{
    public class MessagingConfiguration : IMessagingConfiguration
    {
        private readonly IIntegrationModule _module;
        private readonly int _consumersCount;
        private readonly IDictionary<IIntegrationModule, IMessagingConfigurationListener> _listeners;
        private readonly IDictionary<Type, ListenerConfiguration> _configurations;
        private readonly bool _clearAllQueues = false;

        public MessagingConfiguration(IIntegrationModule module, int consumersCount = 1, bool clearAllQueues = false)
        {
            if (module == null)
                throw new ArgumentNullException(nameof(module));
            _module = module;
            _consumersCount = consumersCount;
            _clearAllQueues = clearAllQueues;
            _listeners = new Dictionary<IIntegrationModule, IMessagingConfigurationListener>();
            _configurations = new Dictionary<Type, ListenerConfiguration>();
        }

        public IDictionary<IIntegrationModule, IMessagingConfigurationListener> GetSubscriptions()
        {
            return _listeners;
        }

        public IMessagingConfigurationListener Subscribe<TModule>() where TModule : IIntegrationModule, new()
        {
            var module = new TModule();
            var listener = new MessagingConfigurationListener();
            _listeners.Add(module, listener);
            return listener;
        }

        public IIntegrationModule GetModule()
        {
            return _module;
        }

        public ListenerConfiguration GetListenerConfigurationFromMessage(Type messageType)
        {
            if (_listeners.Select(x => x.Value).SelectMany(x => x.GetConfigurations()).Any(x => x.MessageType == messageType))
            {
                return _listeners.Select(x => x.Value).SelectMany(x => x.GetConfigurations()).First(x => x.MessageType == messageType);
            }

            List<ListenerConfiguration> configurations = new List<ListenerConfiguration>();
            foreach (var listener in _listeners.Values)
            {
                configurations.AddRange(listener.GetEventActions(ListenerConfiguration.ListernerType.NoResponse));
            }

            var errorMessage = $"Evento {messageType.Name} sem consumidor definido.";
            var listenerConfiguration = configurations.FirstOrDefault(p => p.MessageType == messageType);
            if (listenerConfiguration == null)
                throw new InvalidOperationException(errorMessage);

            _configurations.Add(messageType, listenerConfiguration);
            return listenerConfiguration;
        }

        public int GetConsumersCount()
        {
            return _consumersCount;
        }

        public bool ClearAllQueues()
        {
            return _clearAllQueues;
        }
    }
}
