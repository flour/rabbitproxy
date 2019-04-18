using System;

namespace Shared
{
    public class LogMessage : BaseMessage
    {
        public string Message { get; set; }

        public override MessageTypes MessageType { get => MessageTypes.Log; }
    }
}
