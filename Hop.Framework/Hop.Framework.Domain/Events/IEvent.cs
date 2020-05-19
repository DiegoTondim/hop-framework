using System;

namespace Hop.Framework.Domain.Events
{
    public interface IEvent
    {
        Guid MessageId { get; }
    }
}