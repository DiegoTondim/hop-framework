using System;
using Hop.Framework.Domain.Events;

namespace Hop.Framework.Domain.Commands
{
    public abstract class CommandBase : IEvent
    {
        public Guid MessageId { get; set; }
    }
}