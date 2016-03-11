using Microsoft.AspNet.Antiforgery;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.Attributes
{
    public class AntiforgeryAttribute: AuthorizationFilterAttribute
    {
        private IAntiforgery Antiforgery { get; set; }

        private AntiforgeryMode ForgeryMode { get; set; }

        public AntiforgeryAttribute(AntiforgeryMode forgeryMode = AntiforgeryMode.Default)
        {
            ForgeryMode = forgeryMode;
        }

        public override void OnAuthorization(AuthorizationContext context)
        {
            var httpContext = context.HttpContext;
            if (Antiforgery == null)
                Antiforgery = httpContext.ApplicationServices.GetService(typeof(IAntiforgery)) as IAntiforgery;
            
            var request = httpContext.Request;
            var response = httpContext.Response;
            AntiforgeryTokenSet set;

            if (ForgeryMode != AntiforgeryMode.Set)
            {
                if (!request.Headers.ContainsKey("X-XSRF-TOKEN"))
                {
                    context.Result = new HttpStatusCodeResult(400);
                    return;
                }

                set = new AntiforgeryTokenSet(request.Headers["X-XSRF-TOKEN"], request.Cookies["XSRF"]);
                Antiforgery.ValidateTokens(httpContext, set);
            }

            set = Antiforgery.GetAndStoreTokens(httpContext);
            
            response.Cookies.Append("XSRF-TOKEN", set.FormToken);

            base.OnAuthorization(context);
        }
    }

    public enum AntiforgeryMode
    {
        Default,
        Set
    }
}
