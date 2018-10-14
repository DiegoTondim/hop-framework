using FluentValidation;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Validation;

namespace Hop.Framework.FluentValidation
{
    public class ValidationBase<T> : AbstractValidator<T>, IValidation<T> where T : CommandBase
    {
        public ValidationResult Validate(T command) => base.Validate(command).GetResult();
    }
}
