using Hop.Framework.Core.User;

namespace Hop.Framework.EFCore.Tests.Infra.Context
{
    public class UserContextService : IUserContextService
    {
        public UserContextBase UserContext { get; private set; }

        public UserContextService()
        {
            UserContext = new UserContext();
        }

        public void Set<T>(T context) where T : UserContextBase
        {
            UserContext = context;
        }

        public T Get<T>() where T : UserContextBase
        {
            return UserContext as T;
        }
    }
}