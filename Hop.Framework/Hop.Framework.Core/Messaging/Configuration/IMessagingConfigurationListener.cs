using System;
using System.Collections.Generic;

namespace Hop.Framework.Core.Messaging.Configuration
{
    public interface IMessagingConfigurationListener
    {
        IEnumerable<ListenerConfiguration> GetConfigurations();

        IMessagingConfigurationListener Listen<TEvent, TConsumer>() where TEvent : IMessage where TConsumer : IConsumer<TEvent>;

        IEnumerable<ListenerConfiguration> GetEventActions(ListenerConfiguration.ListernerType listenerType);
    }

    public sealed class ListenerConfiguration
    {
        public enum ListernerType { None = 0, NoResponse = 1, WithResponse = 2 };

        public Type MessageType { get; private set; }
        public Type ConsumerType { get; private set; }
        public Type RequestType { get; private set; }
        public Type ResponseType { get; private set; }
        public ListernerType ListenType { get; private set; }

        public ListenerConfiguration(Type messageType, Type consumerType)
        {
            ListenType = ListernerType.NoResponse;
            MessageType = messageType;
            ConsumerType = consumerType;
        }

        public ListenerConfiguration(Type requestType, Type responseType, Type consumerResponseType)
        {
            ListenType = ListernerType.WithResponse;
            RequestType = requestType;
            ResponseType = responseType;
            ConsumerType = consumerResponseType;
        }
    }
}
