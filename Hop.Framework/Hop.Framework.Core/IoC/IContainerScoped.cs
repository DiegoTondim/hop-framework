using System;

namespace Hop.Framework.Core.IoC
{
    public interface IContainerScoped : IDisposable
    {
    }

    public enum ScopeType
    {
        Thread
    }
}
