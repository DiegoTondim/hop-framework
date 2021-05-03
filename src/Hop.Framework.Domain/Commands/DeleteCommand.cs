namespace Hop.Framework.Domain.Commands
{
    public class DeleteCommand<TPrimaryKeyType> : CommandBase
    {
        public TPrimaryKeyType Id { get; }

        public DeleteCommand(TPrimaryKeyType id)
        {
            Id = id;
        }
    }
}
