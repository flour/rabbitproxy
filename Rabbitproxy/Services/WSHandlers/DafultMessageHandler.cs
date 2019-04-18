using Rabbitproxy.Extensions;
using Rabbitproxy.Services;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Rabbitproxy.WSHandlers.Services
{
    public class DafultMessageHandler : WebSocketHandler
    {
        public DafultMessageHandler(WebSocketManager manager) : base(manager)
        {

        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var message = $"{socket.GetId()} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            await SendMessageAsync(socket, message);
        }
    }
}
