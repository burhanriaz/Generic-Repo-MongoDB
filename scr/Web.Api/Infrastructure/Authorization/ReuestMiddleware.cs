using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using Web.Domain.Models.Account;
using Web.Domain.Service;
using Web.Domain.Service.Abstractions;
using Web.Entity.Entity.Identity;

namespace Web.Api.ServiceCollectionsConfigurations.Authorization;
public class ReuestMiddleware
{
    private readonly RequestDelegate _next;
    public ReuestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IAccountService<UserViewModel, Users> accountService)
    {
        if (httpContext.User.Identity is { IsAuthenticated: true })
        {
            var userSub = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userSub))
            {
                await GenerateUnauthorizedResponse(httpContext);
                return;
            }

            var permissionClaimsList = await accountService.GetUserPermissionClaimsAsync(userSub);
            var permissionClaims = permissionClaimsList.ToArray();

            if (permissionClaims.Length == 0)
            {
                await GenerateUnauthorizedResponse(httpContext);
                return;
            }

            var claimIdentity = CreateClaimIdentityFromPermissions(permissionClaims);
            if (claimIdentity == null)
            {
                await GenerateUnauthorizedResponse(httpContext);
                return;
            }

            httpContext.User.AddIdentity(claimIdentity);
        }

        await _next(httpContext);
    }

    private static async Task GenerateUnauthorizedResponse(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        httpContext.Response.ContentType = "application/json";

        // You can customize the response message as needed
        var responseMessage = new { message = "Unauthorized access" };
        var jsonResponse = JsonConvert.SerializeObject(responseMessage);

        await httpContext.Response.WriteAsync(jsonResponse);
    }


    private static ClaimsIdentity? CreateClaimIdentityFromPermissions(IReadOnlyCollection<Claim> permissionClaims)
    {
        if (!permissionClaims.Any())
        {
            return null;
        }

        var permissionIdentity = new ClaimsIdentity(nameof(ReuestMiddleware), ClaimTypes.Name, ClaimTypes.Role);
        permissionIdentity.AddClaims(permissionClaims);
        return permissionIdentity;
    }
}


//Extension method used to add the middleware to the HTTP request pipeline.
public static class RequestMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ReuestMiddleware>();
    }
}
