using System;
using Hop.Framework.Core.Messaging.Configuration;

namespace Hop.Framework.Core.Messaging.Provider
{
    public interface IMessagingProvider : IDisposable
    {
        IIntegrationModule CurrentModule { get; }
        void Load(IProviderConfiguration providerConfiguration, IMessagingConfiguration messagingConfiguration, bool child = false);
    }
}
