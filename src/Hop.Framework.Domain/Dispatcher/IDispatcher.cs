using Hop.Framework.Core.Messaging;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Results;
using System.Threading.Tasks;

namespace Hop.Framework.Domain.Dispatcher
{
    public interface IDispatcher
    {
        Task<Result> Send(IEvent @event);
        Task<Result> Execute<T>(T command) where T : CommandBase;
    }
}
