using System;
using Hop.Framework.Domain.Models;

namespace Hop.Framework.Domain.Repository
{
    public interface IRepositoryWithGuidKey<TEntity> : IRepository<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {

    }
}