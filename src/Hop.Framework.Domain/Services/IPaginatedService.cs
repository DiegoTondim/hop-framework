using System.Linq.Expressions;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;
using System;
using Hop.Framework.Domain.Models;

namespace Hop.Framework.Domain.Services
{
    public interface IPaginatedService<TReadOnlyViewModel, in TFilter>
        where TFilter : FilterBase
    {
        ResultWithPaginatedData<TReadOnlyViewModel> Filter(TFilter filters);
    }

    public interface IConverterPaginatedService<TReadOnlyViewModel, TEntity, TPrimaryKeyType, TFilter>
        where TFilter : FilterBase
        where TEntity : IEntity<TPrimaryKeyType>
    {
        TReadOnlyViewModel ConvertToPaginatedResponse(TEntity entity);

        Expression<Func<TEntity, bool>> FilterExpression(TFilter filter);
    }
}
