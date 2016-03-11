using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.Attributes
{
    /// <summary>
    /// Forces a 304 'Not Modified'
    /// </summary>
    /// <remarks>
    /// Do NOT use this on dynamic files, as there is no check for changes.
    /// This attribute is only meant to save bandwith.
    /// </remarks>
    public class StaticAttribute : AuthorizationFilterAttribute
    {
        public IMemoryCache Cache { get; set; }

        public override void OnAuthorization(AuthorizationContext context)
        {
            if (Cache == null)
                Cache = context.HttpContext.ApplicationServices.GetRequiredService<IMemoryCache>();
            var last = Cache.Get("ServerStart") as string;

            HttpRequest request = context.HttpContext.Request;
            HttpResponse response = context.HttpContext.Response;
            response.Headers["Last-Modified"] = last;

            if (request.Headers["If-Modified-Since"] == last)
                context.Result = new HttpStatusCodeResult(304);
            base.OnAuthorization(context);
        }
    }
}
