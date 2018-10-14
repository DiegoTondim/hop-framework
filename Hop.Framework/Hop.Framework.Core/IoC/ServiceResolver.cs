namespace Hop.Framework.Core.IoC
{
    public class ServiceResolver
    {
        private static IContainer _container;
        public static IContainer Container => _container;

        public static void Register(IContainer container)
        {
            _container = container;
        }

        public static IContainerScoped BeginScope(ScopeType type)
        {
            return _container.BeginScope(type);
        }

        public static IContainer CreateChildContainer()
        {
            return _container.CreateChildContainer();
        }
    }
}
