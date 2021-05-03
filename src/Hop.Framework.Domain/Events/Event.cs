using Hop.Framework.Core.Messaging;
using System;

namespace Hop.Framework.Domain.Events
{
    public abstract class Event : IEvent
    {
        public DateTime Timestamp { get; private set; }
        public Guid MessageId { get; set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
            MessageId = Guid.NewGuid();
        }
    }
}
