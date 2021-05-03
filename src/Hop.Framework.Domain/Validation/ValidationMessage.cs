using System;

namespace Hop.Framework.Domain.Validation
{
    public class ValidationMessage
    {
        public string Property { get; private set; }
        public string Message { get; private set; }
        public ValidationMessageType Type { get; set; }
        public Guid Id { get; set; }

        private ValidationMessage()
        {
            Id = Guid.NewGuid();
        }

        public ValidationMessage(string property, string message, ValidationMessageType type) : this()
        {
            Message = message;
            Property = property;
            Type = type;
        }

        public bool IsSuccess => Type == ValidationMessageType.Success || Type == ValidationMessageType.Info;

        public override bool Equals(object obj)
        {
            if (!(obj is ValidationMessage other))
            {
                return false;
            }

            return this.Property == other.Property &&
                   this.Message == other.Message;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public enum ValidationMessageType
    {
        Success = 1,
        Info = 2,
        Warning = 3,
        Error = 4
    }
}
