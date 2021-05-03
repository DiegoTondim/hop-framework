namespace Hop.Framework.Core.User
{
    public interface IUserContextService
    {
        UserContextBase UserContext { get; }
        void Set<T>(T userContext) where T : UserContextBase;
        T Get<T>() where T : UserContextBase;
    }
}
