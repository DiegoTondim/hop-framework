using Hop.Framework.Core.User;
using Hop.Framework.EFCore.Context;
using Hop.Framework.EFCore.Repository;
using Hop.Framework.EFCore.Tests.Infra.Models;

namespace Hop.Framework.EFCore.Tests.Infra.Repositories
{
    public class TabelaTesteWithGuidKeyRepository : RepositoryWithGuidKeyBase<TabelaTesteWithGuidKey>
    {
        public TabelaTesteWithGuidKeyRepository(HopContextBase context, IUserContextService userContextService) : base(context, userContextService)
        {
        }
    }
	public class TabelaTesteWithGuidKeySoftDeleteRepository : RepositoryWithGuidKeyBase<TabelaTesteSoftDeleteWithGuidKey>
	{
		public TabelaTesteWithGuidKeySoftDeleteRepository(HopContextBase context, IUserContextService contextService) : base(context, contextService)
		{
		}
	}
}