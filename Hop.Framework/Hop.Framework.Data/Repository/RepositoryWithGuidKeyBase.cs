using System;
using Hop.Framework.EFCore.Context;
using Hop.Framework.Domain.Models;

namespace Hop.Framework.EFCore.Repository
{
    public abstract class RepositoryWithGuidKeyBase<TEntity> : RepositoryBase<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {
        protected RepositoryWithGuidKeyBase(HopContextBase context) : base(context)
        {
        }
    }
}
