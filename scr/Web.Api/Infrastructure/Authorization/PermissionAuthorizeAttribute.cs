using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Authorization;

namespace Web.Api.ServiceCollectionsConfigurations.Authorization;

public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    internal const string PolicyPrefix = "PERMISSION";
    private const string Separator = "_";
    public PermissionAuthorizeAttribute(string permission)
    {
        Policy = $"{PolicyPrefix}{Separator}{permission}";
    }
    public static string GetPermissionFromPolicy(string policyName)
    {
        return policyName.Substring(PolicyPrefix.Length + Separator.Length);
    }

}