using System;

namespace Hop.Framework.Core.Messaging
{
    public interface IErrorLogger
    {
        void Info(string eventName, string error, string exchange, string message, DateTime date);
    }
}
