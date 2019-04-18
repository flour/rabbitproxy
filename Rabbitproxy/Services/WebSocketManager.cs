using Rabbitproxy.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbitproxy.Services
{
    public class WebSocketManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
            => _sockets.FirstOrDefault(s => s.Key == id).Value;

        public IEnumerable<WebSocket> GeAllSockets()
            => _sockets.Select(s => s.Value);

        public bool AddSocket(WebSocket socket)
        {
            socket.SetId(Guid.NewGuid().ToString());
            return _sockets.TryAdd(socket.GetId(), socket);
        }

        public async Task<bool> RemoveSocketAsync(string id)
        {
            if (_sockets.TryRemove(id, out WebSocket socket))
            {
                await socket.CloseAsync(
                    closeStatus: WebSocketCloseStatus.NormalClosure,
                    statusDescription: "Closed by request",
                    cancellationToken: CancellationToken.None
                );
                return true;
            }
            return false;
        }
    }

}
