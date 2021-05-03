using Hop.Framework.Core.User;
using Hop.Framework.EFCore.Context;
using Hop.Framework.EFCore.Repository;
using Hop.Framework.EFCore.Tests.Infra.Models;

namespace Hop.Framework.EFCore.Tests.Infra.Repositories
{
	public class TabelaTesteWithLongKeyRepository : RepositoryWithLongKeyBase<TabelaTesteWithLongKey>
	{
		public TabelaTesteWithLongKeyRepository(HopContextBase context, IUserContextService userContextService) : base(context, userContextService)
		{
		}
	}

	public class TabelaTesteWithLongKeySoftDeleteRepository : RepositoryWithLongKeyBase<TabelaTesteSoftDeleteWithLongKey>
	{
		public TabelaTesteWithLongKeySoftDeleteRepository(HopContextBase context, IUserContextService contextService) : base(context, contextService)
		{
		}
	}
}