namespace Hop.Api.Server.Core.Auth
{
    public enum AuthorizationResultType
    {
        ByPass,
        Failed,
        NotAuthenticated,
        Authenticated,
        NotAuthorized,
        Authorized
    }
}