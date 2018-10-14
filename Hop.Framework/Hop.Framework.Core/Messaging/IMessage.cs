using System;

namespace Hop.Framework.Core.Messaging
{
    public interface IMessage
    {
        Guid Id { get; set; }
    }
}
