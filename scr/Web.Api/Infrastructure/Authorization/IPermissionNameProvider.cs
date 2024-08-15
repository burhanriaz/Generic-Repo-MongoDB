namespace Web.Api.ServiceCollectionsConfigurations.Authorization;
public interface IPermissionNameProvider
{
    public string PermissionPrefix { get; }
    public string GetPermissionName(string policyName);
}