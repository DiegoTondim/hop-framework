using Hop.Net5WebApi.Domain.Commands;

namespace Hop.Net5WebApi.Domain.Validators
{
    public class UpdatePersonValidator : PersonValidator<UpdatePersonCommand>
    {
        public UpdatePersonValidator()
        {
            ValidateName();
        }
    }
}
