using Hop.Framework.Core.Messaging;
using Hop.Framework.Core.Messaging.Configuration;
using System.Collections.Generic;

namespace Hop.Framework.Core.IoC
{
	public interface IModule
	{
		void Load(IContainer container);
		IDictionary<IIntegrationModule, IMessagingConfigurationListener> GetSubscriptions();
	}
}
