using Microsoft.AspNet.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpFora.Security
{
    public class LoginRequirement : AuthorizationHandler<LoginRequirement>, IAuthorizationRequirement
    {
        protected override void Handle(AuthorizationContext context, LoginRequirement requirement)
        {
            context.Succeed(requirement);
        }
    }
}
