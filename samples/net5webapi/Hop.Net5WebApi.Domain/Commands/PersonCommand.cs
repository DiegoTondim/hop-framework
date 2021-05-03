using Hop.Framework.Domain.Commands;

namespace Hop.Net5WebApi.Domain.Commands
{
    public abstract class PersonCommand : CommandBase
    {
        public string Name { get; set; }

        protected PersonCommand(string name)
        {
            Name = name;
        }
    }
}
