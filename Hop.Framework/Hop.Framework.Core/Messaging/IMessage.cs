using System;

namespace Hop.Framework.Core.Messaging
{
    public interface IMessage
    {
        Guid MessageId { get; set; }
    }
}
