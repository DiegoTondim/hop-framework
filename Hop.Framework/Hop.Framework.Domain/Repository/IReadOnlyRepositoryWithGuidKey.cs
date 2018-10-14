using System;
using Hop.Framework.Domain.Models;

namespace Hop.Framework.Domain.Repository
{
    public interface IReadOnlyRepositoryWithGuidKey<TEntity> : IReadOnlyRepository<TEntity, Guid> where TEntity : class, IEntity<Guid>
    {

    }
}