using System;
using System.Security.Claims;
using Hop.Framework.Core.User;

namespace Hop.Api.Server.Core.Auth
{
    public class AuthorizationResult
    {
        public AuthorizationResultType AuthType { get; }
        public UserContextBase Context { get; }
        public string Detail { get; }

        public AuthorizationResult(AuthorizationResultType authType, UserContextBase context = null, string detail = null)
        {
            AuthType = authType;
            Context = context;
            Detail = detail;
        }
    }

    public class MockedUserContext : UserContextBase
    {
        public const string UserGuid = "f0a87ed2-1d90-4cb8-9f6c-b0c6c91292c4";
        public MockedUserContext() : base(Guid.Parse(UserGuid), UserGuid, UserGuid, new Claim[0])
        {
        }
    }
}