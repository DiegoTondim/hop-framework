using System;
using System.Text;

namespace Hop.Framework.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string TraceErrorMessage(Exception e)
        {
            StringBuilder messages = new StringBuilder();
            GetInnerException(e, messages);
            return messages.ToString().Trim();
        }

        public static string ToTraceErrorMessage(this Exception e)
        {
            return TraceErrorMessage(e);
        }

        private static void GetInnerException(Exception e, StringBuilder messages)
        {
            if (e.InnerException != null)
            {
                GetInnerException(e.InnerException, messages);
            }
            messages.AppendLine(e.Message);
        }
    }
}
