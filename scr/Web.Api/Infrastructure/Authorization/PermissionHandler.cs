using Microsoft.AspNetCore.Authorization;
using System.Net;
using Web.Domain;

namespace Web.Api.ServiceCollectionsConfigurations.Authorization
{
    internal class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim(PermissionRequirement.ClaimType, ClaimsConstants.SuperAdminPermission))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (context.User.HasClaim(PermissionRequirement.ClaimType, requirement.Permission))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            // Explicitly set the status code to 401 for unauthorized access
            var httpContext = context.Resource as HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                httpContext.Response.CompleteAsync(); // Ensure response is completed
            }

            context.Fail();
            return Task.CompletedTask;
            // throw new UnauthorizedAccessException("The current user is not authorized to access this resource.");
        }
    }
}
