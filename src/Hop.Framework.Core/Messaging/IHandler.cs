namespace Hop.Framework.Core.Messaging
{
    public interface IHandler<TMessage> where TMessage : IMessage
    {
        void Handle(TMessage message);
    }
}
