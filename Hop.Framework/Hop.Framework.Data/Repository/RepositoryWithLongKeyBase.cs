using Hop.Framework.EFCore.Context;
using Hop.Framework.Domain.Models;
using Hop.Framework.Core.User;

namespace Hop.Framework.EFCore.Repository
{
	public abstract class RepositoryWithLongKeyBase<TEntity> : RepositoryBase<TEntity, long> where TEntity : class, IEntity<long>
	{
		protected RepositoryWithLongKeyBase(HopContextBase context, IUserContextService userContextService) : base(context, userContextService)
		{
		}
	}
}
