using Microsoft.AspNetCore.Authorization;

namespace Web.Api.ServiceCollectionsConfigurations.Authorization;

public static class ServiceCollectionAuthorizationExtensions
{
    public static IServiceCollection AddPermissionAuthorization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IPermissionNameProvider, PermissionNameProvider>();
        // serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddDynamicAuthorization();
        return serviceCollection;
    }
    public static IServiceCollection AddDynamicAuthorization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

        });

        serviceCollection.AddSingleton<IAuthorizationHandler, PermissionHandler>();
        serviceCollection.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        return serviceCollection;
    }

}
