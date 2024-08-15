namespace Web.Api.ServiceCollectionsConfigurations.Authorization
{
    public class PermissionNameProvider : IPermissionNameProvider
    {
        public string PermissionPrefix => PermissionAuthorizeAttribute.PolicyPrefix;

        public string GetPermissionName(string policyName)
        {
            return PermissionAuthorizeAttribute.GetPermissionFromPolicy(policyName);
        }
    }

}
