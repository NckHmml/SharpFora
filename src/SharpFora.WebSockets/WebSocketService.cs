using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using System.Net.WebSockets;

namespace SharpFora.WebSockets
{
    internal class WebSocketService : IWebSocketService
    {
        private ConcurrentHashSet<Client> Clients { get; } = new ConcurrentHashSet<Client>();

        public async Task Handle(HttpContext context, Func<Task> next)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                var client = new Client(socket, (c) => Clients.Remove(c));
                Clients.Add(client);
                await client.Listen();
            }
            else
            {
                await next();
            }
        }

        public async Task SendAll(string message) =>
            await Task.WhenAll(Clients.Select(x => x.SendText(message)));
    }
}
