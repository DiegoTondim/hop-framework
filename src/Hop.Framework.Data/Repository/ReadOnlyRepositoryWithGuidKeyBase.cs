using Hop.Framework.Domain.Models;
using Hop.Framework.EFCore.Context;
using System;

namespace Hop.Framework.EFCore.Repository
{
    public abstract class ReadOnlyRepositoryWithGuidKeyBase<TEntity> : ReadOnlyRepositoryBase<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {
        protected ReadOnlyRepositoryWithGuidKeyBase(HopContextBase context) : base(context)
        {
        }
    }
}
