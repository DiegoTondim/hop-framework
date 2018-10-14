using System.Collections.Generic;
using System.Linq;

namespace Hop.Framework.Domain.Validation
{
    public class ValidationResult
    {
        private readonly IList<ValidationMessage> _messages;

        public IEnumerable<ValidationMessage> Messages => _messages;
        public bool IsValid => !Messages.Any(x => !x.IsSuccess);

        public ValidationResult()
        {
            _messages = new List<ValidationMessage>();
        }

        public void AddMessage(string property, string message, ValidationMessageType type = ValidationMessageType.Error)
        {
            this._messages.Add(new ValidationMessage(property, message, type));
        }
    }
}
