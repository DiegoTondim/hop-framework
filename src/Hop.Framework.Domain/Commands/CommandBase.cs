using Hop.Framework.Core.Messaging;
using System;

namespace Hop.Framework.Domain.Commands
{
	public abstract class CommandBase : IEvent
	{
		public Guid MessageId { get; set; }
	}
}
