using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.WebSockets
{
    public interface IWebSocketService
    {
        Task Handle(HttpContext context, Func<Task> next);
    }
}
