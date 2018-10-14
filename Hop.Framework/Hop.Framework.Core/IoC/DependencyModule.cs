using System;
using System.Threading;

namespace Hop.Framework.Core.IoC
{
    public abstract class DependencyModule : IModule
    {
        public abstract void Load(IContainer container);
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
