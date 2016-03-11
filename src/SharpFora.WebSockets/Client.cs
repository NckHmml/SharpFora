using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpFora.WebSockets
{
    public class Client
    {
        private  WebSocket Socket { get; set; }

        private ArraySegment<byte> Buffer { get; } = new ArraySegment<byte>(new byte[64]);

        private Action<Client> Disconnect { get; set; }

        public Client(WebSocket socket, Action<Client> disconnect)
        {
            Socket = socket;
            Disconnect = disconnect;
        }

        public async Task Listen()
        {
            while (Socket?.State == WebSocketState.Open)
            {
                var received = await Socket.ReceiveAsync(Buffer, CancellationToken.None);
                switch (received.MessageType)
                {
                    case WebSocketMessageType.Close:
                        Disconnect(this);
                        break;
                    default:
                        // We will have the clients send messages through the site itself (RESTful)
                        // So we have no need for a receive function
                        break;
                }
            }
        }

        public async Task SendText(string message)
        {
            var data = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await Socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
