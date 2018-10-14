using System.Collections.Generic;
using Hop.Framework.Domain.Notification;

namespace Hop.Framework.Domain.Results
{
    public class Result<TValue>
    {
        public bool Success { get; private set; }
        private readonly IList<ResultMessage> _messages;
        public IEnumerable<ResultMessage> Messages => _messages;
        public TValue Value { get; private set; }

        protected Result()
        {
            _messages = new List<ResultMessage>();
        }

        protected Result(bool success) : this()
        {
            Success = success;
        }

        public Result(TValue value) : this()
        {
            this.Value = value;
            this.Success = true;
        }

        public Result(TValue value, bool success) : this()
        {
            this.Value = value;
            this.Success = success;
        }

        public void AddMessages(IEnumerable<DomainNotification> messages)
        {
            foreach (var validationMessage in messages)
            {
                this._messages.Add(new ResultMessage(validationMessage.Value, validationMessage.Key));
            }
        }

        public void AddValue(TValue value) => Value = value;
    }

    public class Result : Result<object>
    {
        public static Result Ok => new Result(true);
        public static Result Error => new Result(false);

        protected Result() : base()
        {
        }

        protected Result(bool success) : base(success)
        {
        }
    }
}
