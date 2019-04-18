using Newtonsoft.Json;
using Rabbitproxy.Services;
using Shared;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Rabbitproxy.WSHandlers.Services
{
    public class RabbitMessageHandler : WebSocketHandler
    {
        private readonly IMessageBus _messageBus;

        public RabbitMessageHandler(
            WebSocketManager manager,
            IMessageBus messageBus
        ) : base(manager)
        {
            _messageBus = messageBus;
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = JsonConvert.DeserializeObject<LogMessage>(Encoding.UTF8.GetString(buffer, 0, result.Count));
            IMessage reply;
            try
            {
                reply = await _messageBus.HandleMessageAsync(message);
            }
            catch (System.Exception ex)
            {
                reply = new MessageResponse { Message = ex.Message };
            }
            
            await SendMessageAsync(socket, reply);
        }
    }
}
