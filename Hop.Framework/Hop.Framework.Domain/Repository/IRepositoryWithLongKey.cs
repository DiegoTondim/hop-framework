using Hop.Framework.Domain.Models;

namespace Hop.Framework.Domain.Repository
{
    public interface IRepositoryWithLongKey<TEntity> : IRepository<TEntity, long> where TEntity : class, IEntity<long>
    {

    }
}