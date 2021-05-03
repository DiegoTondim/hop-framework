using System.Collections.Generic;
using Hop.Framework.Domain.Commands;

namespace Hop.Framework.Domain.Validation
{
    public interface IValidation<in TCommand> where TCommand : CommandBase
    {
        ValidationResult Validate(TCommand command);
    }
}
