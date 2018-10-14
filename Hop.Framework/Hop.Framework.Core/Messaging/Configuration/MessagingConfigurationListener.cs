using System.Collections.Generic;
using System.Linq;

namespace Hop.Framework.Core.Messaging.Configuration
{
    public sealed class MessagingConfigurationListener : IMessagingConfigurationListener
    {
        private readonly IList<ListenerConfiguration> _consumers;

        public MessagingConfigurationListener()
        {
            _consumers = new List<ListenerConfiguration>();
        }

        public IEnumerable<ListenerConfiguration> GetEventActions(ListenerConfiguration.ListernerType listenerType)
        {
            if (listenerType == ListenerConfiguration.ListernerType.None)
            {
                return _consumers.ToArray();
            }
            return _consumers.Where(p => p.ListenType == listenerType).ToArray();
        }

        public IEnumerable<ListenerConfiguration> GetConfigurations()
        {
            return _consumers;
        }

        public IMessagingConfigurationListener Listen<TEvent, TConsumer>()
            where TEvent : IMessage
            where TConsumer : IConsumer<TEvent>
        {
            _consumers.Add(new ListenerConfiguration(typeof(TEvent), typeof(TConsumer)));
            return this;
        }
    }
}
