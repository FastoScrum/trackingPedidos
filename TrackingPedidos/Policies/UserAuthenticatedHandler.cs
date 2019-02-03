using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace TrackingPedidos.Policies
{
    public class UserAuthenticatedHandler : AuthorizationHandler<UserAuthenticated>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserAuthenticated requirement)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (requirement == null)
                throw new ArgumentNullException(nameof(requirement));

            var authFilterCtx = (Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)context.Resource;
            var httpContext = authFilterCtx.HttpContext;

            context.Succeed(requirement);

            if (context.User.Identity.IsAuthenticated)
            {
                httpContext.Response.Redirect("/Dashboard");
            }

            return Task.CompletedTask;
        }
    }
}