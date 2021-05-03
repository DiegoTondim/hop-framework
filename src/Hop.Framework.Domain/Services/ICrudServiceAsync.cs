using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;
using System.Threading.Tasks;

namespace Hop.Framework.Domain.Services
{
    public interface ICrudServiceAsync<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TPaginatedResponse> : IPaginatedService<TPaginatedResponse, TFilter>
        where TFilter : FilterBase
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
    {
        Task<Result> GetById(TPrimaryKeyType id);
        Task<Result> Create(TNewCommand command);
        Task<bool> OnCreated(TNewCommand command, Result result);

        Task<Result> Update(TUpdateCommand command);
        Task<bool> OnUpdated(TUpdateCommand command, Result result);

        Task<Result> Remove(TRemoveCommand command);
        Task<bool> OnRemoved(TRemoveCommand command, Result result);

    }
}
