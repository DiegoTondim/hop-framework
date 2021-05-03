using Hop.Framework.Core.User;

namespace Hop.Framework.Core.Messaging
{
    public class RequestInformation : IRequestInformation
    {
        public UserContextBase Context { get; set; }
        public int RetryCount { get; set; }
    }
}
