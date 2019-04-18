using Newtonsoft.Json;
using Rabbitproxy.Extensions;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbitproxy.Services
{
    public abstract class WebSocketHandler
    {
        protected WebSocketManager _manager { get; set; }

        public WebSocketHandler(WebSocketManager manager)
        {
            _manager = manager;
        }

        public virtual Task OnConnectedAsync(WebSocket socket)
        {
            _manager.AddSocket(socket);
            return Task.CompletedTask;
        }

        public virtual async Task<bool> OnDisconnectedAsync(WebSocket socket)
            => await _manager.RemoveSocketAsync(socket.GetId());

        public async Task SendMessageAsync<T>(WebSocket socket, T message)
        {
            if (socket.State != WebSocketState.Open || message == null)
            {
                return;
            }

            var encodedMessage = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            await socket.SendAsync(
                new ArraySegment<byte>(encodedMessage, 0, encodedMessage.Length),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }

        public async Task SendMessageAsync<T>(string socketId, T message)
        {
            await SendMessageAsync(_manager.GetSocketById(socketId), message);
        }

        public async Task SendMessageToAllAsync<T>(T message)
        {
            foreach (var socket in _manager.GeAllSockets())
            {
                if (socket.State == WebSocketState.Open)
                {
                    await SendMessageAsync(socket, message);
                }
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
