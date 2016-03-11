using SharpFora.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharpForaWebSockets(this IServiceCollection services) =>
            services.AddInstance<IWebSocketService>(new WebSocketService());
    }
}
