using Shared;
using System;
using System.Threading.Tasks;

namespace Worker
{
    public class Worker
    {
        public string Name { get; set; }

        public Worker(string name)
        {
            Name = name;
        }

        public Task<IMessage> HandleMessage(IMessage message)
        {
            Console.WriteLine($"Got it by {Name}");
            var response = new MessageResponse { Message = $"Got it by {Name}" };
            return Task.FromResult<IMessage>(response);
        }
    }
}
