using System;
using System.Security.Claims;
using Hop.Framework.Core.User;

namespace Hop.Framework.EFCore.Tests.Infra.Context
{
    public class UserContext : UserContextBase
    {
        public const string UserGuid = "f0a87ed2-1d90-4cb8-9f6c-b0c6c91292c4";
        public UserContext() : base(Guid.Parse(UserGuid), UserGuid, UserGuid, new Claim[0])
        {
        }
    }
}