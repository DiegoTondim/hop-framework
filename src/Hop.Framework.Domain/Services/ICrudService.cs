using System.Threading.Tasks;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Repository;
using Hop.Framework.Domain.Results;

namespace Hop.Framework.Domain.Services
{
    public interface ICrudService<TNewCommand, TUpdateCommand, TRemoveCommand, TPrimaryKeyType, TFilter, TResponse, TPaginatedResponse> : IPaginatedService<TPaginatedResponse, TFilter>
        where TFilter : FilterBase
        where TNewCommand : CommandBase
        where TUpdateCommand : CommandBase
        where TRemoveCommand : DeleteCommand<TPrimaryKeyType>
    {
        Result GetById(TPrimaryKeyType id);
        Result Create(TNewCommand command);
        Task<bool> OnCreated(TNewCommand command, Result result);

        Result Update(TUpdateCommand command);
        Task<bool> OnUpdated(TUpdateCommand command, Result result);

        Result Remove(TRemoveCommand command);
        Task<bool> OnRemoved(TRemoveCommand command, Result result);
    }
}
