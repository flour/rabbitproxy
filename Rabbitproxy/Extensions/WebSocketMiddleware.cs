using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Rabbitproxy.Services;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbitproxy.Extensions
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler _wsHandler { get; set; }

        public WebSocketMiddleware(RequestDelegate next, WebSocketHandler wsHandler)
        {
            _next = next;
            _wsHandler = wsHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws" && !context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
            }
            else
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                await _wsHandler.OnConnectedAsync(socket);
                await HandleRequestAsync(socket, async (result, buffer) =>
                {
                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Close:
                            await _wsHandler.OnDisconnectedAsync(socket);
                            break;
                        case WebSocketMessageType.Text:
                            await _wsHandler.ReceiveAsync(socket, result, buffer);
                            break;
                        default:
                            break;
                    }
                });
            }
        }

        private async Task HandleRequestAsync(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                    buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None
                );
                handleMessage(result, buffer);
            }
        }
    }

    public static class CustomHandlerExtensions
    {
        public static IApplicationBuilder UseCustomWSHandler
        (
            this IApplicationBuilder builder,
            PathString path,
            WebSocketHandler handler
        )
        {
            return builder
                .UseWebSockets()
                .Map(path, app => app.UseMiddleware<WebSocketMiddleware>(handler));
        }
    }
}
