namespace Hop.Framework.Core.Messaging
{
    public interface IConsumer<TMessage> where TMessage : IMessage
    {
        void Consume(TMessage message);
    }
}
