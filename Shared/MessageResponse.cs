using System;

namespace Shared
{
    public class MessageResponse : BaseMessage
    {
        public string Message { get; set; }

        public override MessageTypes MessageType { get => MessageTypes.Response; }
    }
}
