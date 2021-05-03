using Hop.Framework.Core.User;

namespace Hop.Framework.Core.Messaging
{
    public interface IRequestInformation
    {
        UserContextBase Context { get; set; }
        int RetryCount { get; set; }
    }
}
