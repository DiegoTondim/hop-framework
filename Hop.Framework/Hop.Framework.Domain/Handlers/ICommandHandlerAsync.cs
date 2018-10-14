using System.Threading.Tasks;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Results;

namespace Hop.Framework.Domain.Handlers
{
    public interface ICommandHandlerWithResultAsync<in T> where T : CommandBase
    {
        Task<Result> Execute(T command);
    }
}
