using Hop.Framework.Domain.Models;
using Hop.Framework.EFCore.Context;

namespace Hop.Framework.EFCore.Repository
{
    public abstract class ReadOnlyRepositoryWithLongKeyBase<TEntity> : ReadOnlyRepositoryBase<TEntity, long> where TEntity : class, IEntity<long>
    {
        protected ReadOnlyRepositoryWithLongKeyBase(HopContextBase context) : base(context)
        {
        }
    }
}
