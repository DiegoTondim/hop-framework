using System;

namespace Hop.Framework.Core.IoC
{
	public interface IContainer : IDisposable
	{
		void UseThreadLifestyle();
		void Register(Type from, Type to, Lifestyle lifestyle);
		void Register(Type from, Func<IContainer, object> to, Lifestyle lifestyle);
		void Register<TFrom, TTo>(Lifestyle lifestyle = Lifestyle.Transient) where TTo : TFrom;
		void Register<TFrom>(Func<IContainer, object> to, Lifestyle lifestyle = Lifestyle.Transient);
		void Load(DependencyModule module);
		T Resolve<T>();
		object Resolve(Type t);

		T ResolveOrDefault<T>();
		object ResolveOrDefault(Type t);

		IContainerScoped BeginScope(ScopeType type);
		IContainer CreateChildContainer();
	}

	public enum Lifestyle
	{
		Singleton,
		Transient,
		Scoped
	}
}
