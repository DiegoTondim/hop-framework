using Hop.Framework.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Hop.Framework.Domain.Repository
{
    public interface IRepository<TEntity, TPrimaryKey> : IReadOnlyRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        TEntity AddOrUpdate(TEntity entity);
        Task<TEntity> AddOrUpdateAsync(TEntity entity);

        void Remove(TPrimaryKey id);
        Task<TEntity> RemoveAsync(TPrimaryKey id);

        void Remove(TEntity entity);
        Task<TEntity> RemoveAsync(TEntity entity);
    }
}
