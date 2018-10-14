using Hop.Framework.Domain.Models;

namespace Hop.Framework.Domain.Repository
{
    public interface IReadOnlyRepositoryWithLongKey<TEntity> : IReadOnlyRepository<TEntity, long> where TEntity : class, IEntity<long>
    {

    }
}