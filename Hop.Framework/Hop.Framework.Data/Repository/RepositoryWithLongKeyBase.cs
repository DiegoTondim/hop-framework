using Hop.Framework.EFCore.Context;
using Hop.Framework.Domain.Models;

namespace Hop.Framework.EFCore.Repository
{
    public abstract class RepositoryWithLongKeyBase<TEntity> : RepositoryBase<TEntity, long> where TEntity : class, IEntity<long>
    {
        protected RepositoryWithLongKeyBase(HopContextBase context) : base(context)
        {
        }
    }
}
