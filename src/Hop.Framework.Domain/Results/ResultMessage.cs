namespace Hop.Framework.Domain.Results
{
    public class ResultMessage
    {
        public string Message { get; set; }
        public string Type { get; set; }

        public ResultMessage(string message, string type)
        {
            Message = message;
            Type = type;
        }
    }
}
