using Hop.Framework.Core.Messaging;
using Hop.Framework.Core.Messaging.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hop.Framework.Core.IoC
{
    public abstract class DependencyModule : IModule
    {
		private readonly IDictionary<IIntegrationModule, IMessagingConfigurationListener> _listeners;
		protected DependencyModule()
		{
			_listeners = new Dictionary<IIntegrationModule, IMessagingConfigurationListener>();
		}

		public abstract void Load(IContainer container);
		public void AutoLoadModules<TModule>(IContainer container) where TModule : DependencyModule
		{
			var implementedConfigTypes = typeof(TModule).Assembly
				.GetTypes()
				.Where(t => !t.IsAbstract
					&& !t.IsGenericTypeDefinition && t.IsSubclassOf(typeof(DependencyModule)) && t.FullName != typeof(TModule).FullName);
			foreach (var item in implementedConfigTypes)
			{
				var module = (DependencyModule)Activator.CreateInstance(item);
				container.Load(module);
				foreach (var subs in module.GetSubscriptions())
				{
					_listeners.Add(subs);
				}
			}
		}

		protected IMessagingConfigurationListener Subscribe<TModule>() where TModule : IIntegrationModule, new()
		{
			var module = new TModule();
			var listener = new MessagingConfigurationListener();
			_listeners.Add(module, listener);
			return listener;
		}

		public IDictionary<IIntegrationModule, IMessagingConfigurationListener> GetSubscriptions()
		{
			return _listeners;
		}
	}

    public class Instance
    {
        public Type Concrete { get; set; }
        public Func<object> InstanceCreator { get; set; }
        public Lifestyle Lifestyle { get; set; }

        public Instance(Type concreta, Lifestyle lifestyle, Func<object> instanceCreator)
        {
            Concrete = concreta;
            Lifestyle = lifestyle;
            InstanceCreator = instanceCreator;
        }

        private Instance() { }

        public static Instance New(Func<object> instanceCreator, Lifestyle lifestyle)
        {
            return new Instance()
            {
                InstanceCreator = instanceCreator,
                Lifestyle = lifestyle
            };
        }
    }
}
