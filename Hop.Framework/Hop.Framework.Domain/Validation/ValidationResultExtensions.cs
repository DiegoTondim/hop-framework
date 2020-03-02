using FluentValidation;
using Hop.Framework.Domain.Validation;

namespace Hop.Framework.Domain.Validation
{
    public static class ValidationResultExtensions
    {
        public static ValidationResult GetResult(this global::FluentValidation.Results.ValidationResult result)
        {
            var finalResult = new ValidationResult();
            foreach (var message in result.Errors)
            {
                finalResult.AddMessage(message.PropertyName, message.ErrorMessage, Convert(message.Severity));
            }
            return finalResult;
        }

        private static ValidationMessageType Convert(Severity severity)
        {
            switch (severity)
            {
                case Severity.Error:
                    return ValidationMessageType.Error;
                case Severity.Info:
                    return ValidationMessageType.Info;
                default:
                    return ValidationMessageType.Warning;
            }
        }
    }
}
