using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Hop.Framework.EFCore.Context;
using Hop.Framework.Domain.Models;
using Hop.Framework.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hop.Framework.EFCore.Repository
{
    public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly HopContextBase _context;

        protected RepositoryBase(HopContextBase context)
        {
            _context = context;
        }

        public TEntity AddOrUpdate(TEntity entity)
        {
            EntityEntry<TEntity> entry;
            if (entity.Id == null || EqualityComparer<TPrimaryKey>.Default.Equals(entity.Id, default(TPrimaryKey)))
            {
                entry = _context.Set<TEntity>().Add(entity);
            }
            else
            {
                var entityContext = _context.Set<TEntity>();
                var objToUpdate = entityContext.Find(entity.Id);
                if (objToUpdate == null)
                {
                    throw new Exception("Cannot update an entity that does not exists in store.");
                }

                entry = _context.Entry(objToUpdate);
                entry.CurrentValues.SetValues(entity);
                entry.State = EntityState.Modified;
            }

            return entry.Entity;
        }

        public async Task<TEntity> AddOrUpdateAsync(TEntity entity)
        {
            EntityEntry<TEntity> entry;
            if (entity.Id == null || EqualityComparer<TPrimaryKey>.Default.Equals(entity.Id, default(TPrimaryKey)))
            {
                entry = await _context.Set<TEntity>().AddAsync(entity);
            }
            else
            {
                var entityContext = _context.Set<TEntity>();
                var objToUpdate = await entityContext.FindAsync(entity.Id);
                if (objToUpdate == null)
                {
                    throw new Exception("Cannot update an entity that does not exists in store.");
                }

                entry = _context.Entry(objToUpdate);
                entry.CurrentValues.SetValues(entity);
                entry.State = EntityState.Modified;
            }

            return await Task.FromResult(entry.Entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Limited to 500 records</returns>
        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return GetSetOfAllWithIncludes().Take(RepositoryConsts.MaxFindAllRecords).Where(predicate);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>Limited to 500 records</returns>
        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetSetOfAllWithIncludes().Take(RepositoryConsts.MaxFindAllRecords).Where(predicate).ToListAsync();
        }

        public IQueryable<TEntity> GetAll()
        {
            return GetSetOfAllWithIncludes();
        }

        public TEntity GetById(TPrimaryKey id)
        {
            return GetSetOfOneWithIncludes().FirstOrDefault(x => x.Id.Equals(id));
        }

        protected virtual IQueryable<TEntity> ConfigureGetAllIncludes(IQueryable<TEntity> query)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> ConfigureGetByIdIncludes(IQueryable<TEntity> query)
        {
            return query;
        }
        public async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return await GetSetOfOneWithIncludes().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        private IQueryable<TEntity> GetSetOfAllWithIncludes()
        {
            return ConfigureGetAllIncludes(_context.Set<TEntity>());
        }

        private IQueryable<TEntity> GetSetOfOneWithIncludes()
        {
            return ConfigureGetByIdIncludes(_context.Set<TEntity>());
        }

        public void Remove(TPrimaryKey id)
        {
            _context.Set<TEntity>().Remove(_context.Set<TEntity>().Find(id));
        }

        public async Task<TEntity> RemoveAsync(TPrimaryKey id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return await Task.FromResult(_context.Set<TEntity>().Remove(entity)?.Entity);
        }

        public void Remove(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<TEntity> RemoveAsync(TEntity entity)
        {
            return await Task.FromResult(_context.Set<TEntity>().Remove(entity)?.Entity);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
