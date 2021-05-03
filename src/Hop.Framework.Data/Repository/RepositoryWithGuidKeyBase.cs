using System;
using Hop.Framework.EFCore.Context;
using Hop.Framework.Domain.Models;
using Hop.Framework.Core.User;
using Hop.Framework.Domain.Repository;

namespace Hop.Framework.EFCore.Repository
{
    public abstract class RepositoryWithGuidKeyBase<TEntity> : RepositoryBase<TEntity, Guid>, IRepositoryWithGuidKey<TEntity> where TEntity : class, IEntity<Guid>
    {
        protected RepositoryWithGuidKeyBase(HopContextBase context, IUserContextService userContextService) : base(context, userContextService)
        {
        }
    }
}
