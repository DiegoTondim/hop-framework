using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Results;

namespace Hop.Framework.Domain.Handlers
{
    public interface ICommandHandler<in T> where T : CommandBase
    {
        void Execute(T command);
    }

    public interface ICommandHandlerWithResult<in T> where T : CommandBase
    {
        Result Execute(T command);
    }
}
