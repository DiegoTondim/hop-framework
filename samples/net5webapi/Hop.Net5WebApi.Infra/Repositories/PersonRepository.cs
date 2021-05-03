using Hop.Framework.Core.User;
using Hop.Framework.EFCore.Context;
using Hop.Framework.EFCore.Repository;
using Hop.Net5WebApi.Domain.Entities;

namespace Hop.Net5WebApi.Infra.Repositories
{
    public class PersonRepository : RepositoryWithGuidKeyBase<PersonEntity>
    {
        public PersonRepository(HopContextBase context, IUserContextService userContextService)
            : base(context, userContextService)
        {
        }
    }
}
