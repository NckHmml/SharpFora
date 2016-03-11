using Microsoft.AspNet.Builder;
using SharpFora.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Builder
{
    public static class BuilderExtensions
    {
        public static void UseSharpForaWebSockets(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            var service = app.ApplicationServices.GetService(typeof(IWebSocketService)) as IWebSocketService;
            app.Use(async (context, next) => await service.Handle(context, next));
        }
    }
}
