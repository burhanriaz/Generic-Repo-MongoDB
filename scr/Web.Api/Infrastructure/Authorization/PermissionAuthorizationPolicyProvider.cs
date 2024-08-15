using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Web.Api.ServiceCollectionsConfigurations.Authorization;

internal class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly IPermissionNameProvider _permissionNameProvider;

    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, IPermissionNameProvider permissionNameProvider) : base(options)
    {
        _permissionNameProvider = permissionNameProvider ?? throw new ArgumentNullException(nameof(permissionNameProvider));
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {

        if (!policyName.StartsWith(_permissionNameProvider.PermissionPrefix, StringComparison.OrdinalIgnoreCase))
        {
            return await base.GetPolicyAsync(policyName);
        }

        var permission = _permissionNameProvider.GetPermissionName(policyName);



        var requirement = new PermissionRequirement(permission);
        return new AuthorizationPolicyBuilder()
            .AddRequirements(requirement).Build();

    }
}
