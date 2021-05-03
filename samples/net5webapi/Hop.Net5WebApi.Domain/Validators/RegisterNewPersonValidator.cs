using Hop.Net5WebApi.Domain.Commands;

namespace Hop.Net5WebApi.Domain.Validators
{
    public class RegisterNewPersonValidator : PersonValidator<RegisterNewPersonCommand>
    {
        public RegisterNewPersonValidator()
        {
            ValidateName();
        }
    }
}
