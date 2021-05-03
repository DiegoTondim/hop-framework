namespace Hop.Net5WebApi.Domain.Commands
{
    public class RegisterNewPersonCommand : PersonCommand
    {

        public RegisterNewPersonCommand(string name) : base(name)
        {
        }
    }
}
