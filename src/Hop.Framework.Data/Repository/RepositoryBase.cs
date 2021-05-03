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
using Hop.Framework.Core.User;

namespace Hop.Framework.EFCore.Repository
{
	public abstract class RepositoryBase<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
	{
		private readonly HopContextBase _context;
		private readonly IUserContextService _contextService;
		private bool _filterRemovedEntities = true;

		protected RepositoryBase(HopContextBase context, IUserContextService contextService)
		{
			_context = context;
			_contextService = contextService;
		}

		public virtual string UpdateUnexistingEntityMessage => "Cannot update an entity that does not exists in store.";

		public void DisableFilterByRemovedEntities() => _filterRemovedEntities = false;
		public void EnableFilterByRemovedEntities() => _filterRemovedEntities = true;

		public virtual TEntity AddOrUpdate(TEntity entity)
		{
			EntityEntry<TEntity> entry;
			if (entity.Id == null || EqualityComparer<TPrimaryKey>.Default.Equals(entity.Id, default(TPrimaryKey)))
			{
				entry = Insert(entity);
			}
			else
			{
				entry = this.Update(entity);
			}

			return entry.Entity;
		}

		private EntityEntry<TEntity> Insert(TEntity entity)
		{
			return _context.Set<TEntity>().Add(entity);
		}

		public virtual async Task<TEntity> AddOrUpdateAsync(TEntity entity)
		{
			EntityEntry<TEntity> entry;
			if (entity.Id == null || EqualityComparer<TPrimaryKey>.Default.Equals(entity.Id, default(TPrimaryKey)))
			{
				entry = await InsertAsync(entity);
			}
			else
			{
				entry = this.Update(entity);
			}

			return await Task.FromResult(entry.Entity);
		}

		private async Task<EntityEntry<TEntity>> InsertAsync(TEntity entity)
		{
			return await _context.Set<TEntity>().AddAsync(entity);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns>Limited to 500 records</returns>
		public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
		{
			return GetSetOfAllWithIncludes().Take(RepositoryConsts.MaxFindAllRecords).Where(predicate);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns>Limited to 500 records</returns>
		public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await GetSetOfAllWithIncludes().Take(RepositoryConsts.MaxFindAllRecords).Where(predicate).ToListAsync();
		}

		public virtual IQueryable<TEntity> GetAll()
		{
			return GetSetOfAllWithIncludes();
		}

		public virtual TEntity GetById(TPrimaryKey id)
		{
			return GetSetOfOneWithIncludes().FirstOrDefault(x => x.Id.Equals(id));
		}

		private IQueryable<TEntity> FilterBySoftDelete(IQueryable<TEntity> query)
		{
			if (_filterRemovedEntities)
			{
				if (typeof(TEntity).GetInterfaces().Any(x => x == typeof(ISoftDelete)))
				{
					Expression<Func<TEntity, bool>> getActive = x => !((ISoftDelete)x).Removed;
					getActive = (Expression<Func<TEntity, bool>>)SoftDeleteVisitor.Visit(getActive);
					return query.Where(getActive);
				}
			}
			return query;
		}

		protected virtual IQueryable<TEntity> ConfigureGetAllIncludes(IQueryable<TEntity> query)
		{
			return query;
		}

		protected virtual IQueryable<TEntity> ConfigureGetByIdIncludes(IQueryable<TEntity> query)
		{
			return query;
		}
		public virtual async Task<TEntity> GetByIdAsync(TPrimaryKey id)
		{
			return await GetSetOfOneWithIncludes().FirstOrDefaultAsync(x => x.Id.Equals(id));
		}

		private IQueryable<TEntity> GetSetOfAllWithIncludes()
		{
			return FilterBySoftDelete(ConfigureGetAllIncludes(_context.Set<TEntity>()));
		}

		private IQueryable<TEntity> GetSetOfOneWithIncludes()
		{
			return FilterBySoftDelete(ConfigureGetByIdIncludes(_context.Set<TEntity>()));
		}

		public virtual string RemoveUnexistingEntityMessage => "Cannot remove an entity that does not exists in store.";

		public virtual void Remove(TPrimaryKey id)
		{
			var entity = GetById(id);
			if (entity == null)
			{
				throw new Exception(RemoveUnexistingEntityMessage);
			}

			this.Remove(entity);
		}

		public virtual async Task<TEntity> RemoveAsync(TPrimaryKey id)
		{
			var entity = await GetByIdAsync(id);
			if (entity == null)
			{
				throw new Exception(RemoveUnexistingEntityMessage);
			}

			return await this.RemoveAsync(entity);
		}

		public virtual void Remove(TEntity entity)
		{
			if (entity != null)
			{
				if (entity is ISoftDelete)
					(entity as ISoftDelete).Remove(_contextService?.UserContext?.Name);
				else
					_context.Set<TEntity>().Remove(entity);
			}
		}

		public virtual async Task<TEntity> RemoveAsync(TEntity entity)
		{
			if (entity != null)
			{
				if (entity is ISoftDelete)
				{
					(entity as ISoftDelete).Remove(_contextService?.UserContext?.Name);
					return await Task.FromResult(entity);
				}
				else
					return await Task.FromResult(_context.Set<TEntity>().Remove(entity)?.Entity);
			}
			return null;
		}

		public virtual TEntity Add(TEntity entity)
		{
			return Insert(entity)?.Entity;
		}

		public virtual async Task<TEntity> AddAsync(TEntity entity)
		{
			return (await InsertAsync(entity))?.Entity;
		}

		private EntityEntry<TEntity> Update(TEntity entity)
		{
			var localEntity = _context.Set<TEntity>().Local.Any(x => x.Id.Equals(entity.Id));
			EntityEntry<TEntity> entry = null;
			if (localEntity)
			{
				var objToUpdate = _context.Set<TEntity>().Find(entity.Id);
				entry = _context.Entry(objToUpdate);
				entry.CurrentValues.SetValues(entity);
				entry.State = EntityState.Modified;
			}
			else
			{
				entry = _context.Set<TEntity>().Update(entity);
			}
			return entry;
		}
	}
}
