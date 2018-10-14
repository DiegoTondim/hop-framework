using System;

namespace Hop.Framework.Core.Log
{
    public interface ILogProvider
    {
        void Trace(string content);
        void Debug(string content);
        void Info(string content);
        void Warning(string content);
        void Error(string content);
        void Error(string content, Exception exception);
        void Fatal(string content);
    }
}
