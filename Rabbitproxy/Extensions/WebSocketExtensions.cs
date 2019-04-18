using System;
using System.Net.WebSockets;

namespace Rabbitproxy.Extensions
{
    public static class WebSocketExtensions
    {
        private static string _socketId = Guid.NewGuid().ToString();

        public static void SetId(this WebSocket socket, string id)
        {
            _socketId = id;
        }

        public static string GetId(this WebSocket socket)
        {
            return _socketId;
        }
    }
}
