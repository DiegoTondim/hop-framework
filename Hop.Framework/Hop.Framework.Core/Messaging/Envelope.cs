using System;

namespace Hop.Framework.Core.Messaging
{
    public class Envelope : ICloneable
    {
        public RequestInformation RequestInformation { get; set; }
        public IMessage Message { get; set; }

        public Envelope()
        {
        }

        public Envelope(RequestInformation requestInformation, IMessage message) : this()
        {
            RequestInformation = requestInformation;
            Message = message;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
