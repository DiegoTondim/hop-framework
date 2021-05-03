using Hop.Framework.Domain.Models;
using Hop.Framework.Domain.Results;
using System.Threading.Tasks;

namespace Hop.Framework.Domain.Services
{
    public interface ILookUpService<TFilter, TResponse, TEntity, TPrimaryKeyType> :
        ILookUpService<TFilter>
    where TEntity : IEntity<TPrimaryKeyType>
    {
        TResponse ConvertToLookupResponse(TEntity entity);
    }

    public interface ILookUpService<TFilter>
    {
        Result LookupFilter(TFilter filter);
    }

    public interface ILookUpServiceAsync<TFilter>
    {
        Task<Result> LookupFilter(TFilter filter);
    }

    public interface ILookUpServiceAsync<TEntity, TFilter, TResponse, TPrimaryKeyType> :
        ILookUpServiceAsync<TFilter>
        where TEntity : IEntity<TPrimaryKeyType>
    {
        Task<Result> LookupFilter(TFilter filter);
        TResponse ConvertToLookupResponse(TEntity entity);
    }
}
