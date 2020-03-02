using FluentValidation;
using Hop.Framework.Domain.Commands;
using Hop.Framework.Domain.Validation;

namespace Hop.Framework.Domain.Validation
{
    public class ValidationBase<T> : AbstractValidator<T>, IValidation<T> where T : CommandBase
    {
        public ValidationResult Validate(T command) => base.Validate(command).GetResult();
    }
}
