using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hop.Framework.Domain.Repository
{
    public interface IReadOnlyRepository<TEntity, TPrimaryKey> where TEntity : class
    {
        TEntity GetById(TPrimaryKey id);
        Task<TEntity> GetByIdAsync(TPrimaryKey id);
        IQueryable<TEntity> GetAll();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    }
}