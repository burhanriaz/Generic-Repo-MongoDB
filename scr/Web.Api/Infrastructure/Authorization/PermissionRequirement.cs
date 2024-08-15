using Microsoft.AspNetCore.Authorization;
using Web.Domain;

namespace Web.Api.ServiceCollectionsConfigurations.Authorization;
//For more explanation
//https://blog.joaograssi.com/posts/2021/asp-net-core-protecting-api-endpoints-with-dynamic-policies/
internal class PermissionRequirement : IAuthorizationRequirement
{
    public static string ClaimType => ClaimsConstants.PermissionClaim;
    public string Permission { get; }

    public PermissionRequirement(string permission)
    {
        if (string.IsNullOrWhiteSpace(permission))
            throw new ArgumentException(nameof(permission));
        Permission = permission;
    }
}
