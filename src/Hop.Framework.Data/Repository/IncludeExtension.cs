using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Hop.Framework.EFCore.Repository
{
    public static class IncludeExtension
    {
        public static IQueryable<TEntity> IncludeProperty<TEntity>(this DbSet<TEntity> dbSet,
                                                params Expression<Func<TEntity, object>>[] includes)
                                                where TEntity : class
        {
            IQueryable<TEntity> query = null;
            foreach (var include in includes)
            {
                query = (query ?? dbSet).Include(include);
            }

            return query ?? dbSet;
        }

        public static IQueryable<TEntity> IncludeProperty<TEntity>(this IQueryable<TEntity> dbSet,
                                                params Expression<Func<TEntity, object>>[] includes)
                                                where TEntity : class
        {
            foreach (var include in includes)
            {
                dbSet = dbSet.Include(include);
            }

            return dbSet;
        }
    }
}
