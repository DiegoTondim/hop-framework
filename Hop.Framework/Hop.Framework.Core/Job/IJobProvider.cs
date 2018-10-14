using System;
using System.Collections.Generic;
using System.Text;

namespace Hop.Framework.Core.Job
{
    public interface IJobProvider<T> : IDisposable
    {
        string JobName { get; }
        T WorkActioParam { get; }
        Action<T> WorkAction { get; }
        DateTime LastRunTime { get; }
        int Interval { get; }
        bool StopRequested { get; }
        void Start(bool runImmediately = false);
        void Stop();
    }
}
