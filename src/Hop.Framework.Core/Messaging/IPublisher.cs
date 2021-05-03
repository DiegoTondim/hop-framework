using System;

namespace Hop.Framework.Core.Messaging
{
    public interface IPublisher : IDisposable
    {
        void Publish<TEvent>(TEvent @event) where TEvent : class, IMessage;
        void PublishToModule<TModule, TEvent>(TEvent @event) where TModule : class, IIntegrationModule, new() where TEvent : class, IMessage;
        void PublishPendingMessages();
    }

    public interface IInternalPublisher : IPublisher
    {
    }
}
