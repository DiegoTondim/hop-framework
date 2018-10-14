using Hop.Framework.Domain.Models;
using Hop.Framework.Domain.Repository;
using Hop.Framework.EFCore.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hop.Framework.EFCore.Repository
{
    public abstract class ReadOnlyRepositoryBase<TEntity, TPrimaryKey> : IReadOnlyRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly HopContextBase _context;

        protected ReadOnlyRepositoryBase(HopContextBase context)
        {
            _context = context;
        }

        public TEntity GetById(TPrimaryKey id)
        {
            return GetSetOfOneWithIncludes().AsNoTracking().FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return await GetSetOfOneWithIncludes().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public IQueryable<TEntity> GetAll()
        {
            return GetSetOfAllWithIncludes().AsNoTracking();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSetOfAllWithIncludes().Take(RepositoryConsts.MaxFindAllRecords).AsNoTracking().Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetSetOfAllWithIncludes().Take(RepositoryConsts.MaxFindAllRecords).AsNoTracking().Where(predicate).ToListAsync();
        }

        protected virtual IQueryable<TEntity> ConfigureGetAllIncludes(IQueryable<TEntity> query)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> ConfigureGetByIdIncludes(IQueryable<TEntity> query)
        {
            return query;
        }

        private IQueryable<TEntity> GetSetOfAllWithIncludes()
        {
            return ConfigureGetAllIncludes(_context.Set<TEntity>());
        }

        private IQueryable<TEntity> GetSetOfOneWithIncludes()
        {
            return ConfigureGetByIdIncludes(_context.Set<TEntity>());
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
