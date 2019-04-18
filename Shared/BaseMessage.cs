using System;

namespace Shared
{
    public interface IMessage
    {
        MessageTypes MessageType { get; }
        DateTime Created { get; set; }
    }

    public class BaseMessage : IMessage
    {
        public virtual MessageTypes MessageType { get => MessageTypes.None; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
