using FluentValidation;
using Hop.Framework.FluentValidation;
using Hop.Net5WebApi.Domain.Commands;

namespace Hop.Net5WebApi.Domain.Validators
{
    public class PersonValidator<T>
        : ValidationBase<T> where T : PersonCommand
    {
        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Name")
                .Must(x => x != "James").WithMessage("James is not a good Name!").WithSeverity(Severity.Info)
                .Length(2, 150).WithMessage("The Name must have between 2 and 150 characters")
                .WithSeverity(Severity.Error);
        }
    }
}
