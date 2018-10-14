using System;
using System.Collections.Generic;
using System.Linq;
using Hop.Framework.Domain.Results;

namespace Hop.Framework.Domain.Repository
{
    public static class RepositoryExtensions
    {
        public static PaginatedData<TViewModel> Paginate<TEntity, TViewModel>(this IEnumerable<TEntity> data, int perPage, int current, Func<TEntity, TViewModel> converter)
        {
            var filteredData = data.Skip((current - 1) * perPage).Take(perPage).ToList();
            return new PaginatedData<TViewModel>(current, data.Count(), perPage, filteredData.Select(converter));
        }

        public static IQueryable<TEntity> Order<TEntity>(this IQueryable<TEntity> data, Order order, Func<TEntity, object> function)
        {
            if (function == null) return data;

            var sortedData = order == Repository.Order.Asc ? data.OrderBy(function) : data.OrderByDescending(function);
            return sortedData.AsQueryable();
        }
    }
}
