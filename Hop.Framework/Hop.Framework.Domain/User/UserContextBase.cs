using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Hop.Framework.Domain.User
{
    public abstract class UserContextBase
    {
        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<Claim> Claims { get; private set; }

        protected UserContextBase(Guid id, string login, string name, IEnumerable<Claim> claims)
        {
            Id = id;
            Login = login;
            Name = name;
            Claims = claims;
        }
    }
}
