using Shared;
using System.Collections.Generic;

namespace Rabbitproxy.Services
{
    public class LogStorage
    {
        public List<LogMessage> Logs { get; set; } = new List<LogMessage>();
    }
}
