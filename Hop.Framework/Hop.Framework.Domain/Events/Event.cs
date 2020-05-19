using System;

namespace Hop.Framework.Domain.Events
{
    public abstract class Event : IEvent
    {
        public DateTime Timestamp { get; }
        public Guid MessageId { get; set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
            MessageId = Guid.NewGuid();
        }
    }
}
