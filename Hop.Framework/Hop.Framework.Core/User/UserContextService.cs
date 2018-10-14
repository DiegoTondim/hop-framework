namespace Hop.Framework.Core.User
{
    public class UserContextService : IUserContextService
    {
        public UserContextBase UserContext { get; private set; }
        public void Set<T>(T userContext) where T : UserContextBase
        {
            UserContext = userContext;
        }

        public T Get<T>() where T : UserContextBase
        {
            return (T)UserContext;
        }
    }
}
