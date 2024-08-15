using Web.Api.Utils.JWTTokenGenerator;
using Web.Domain.Service.Abstractions.Generic;
using Web.Domain.Service.Abstractions;
using Web.Domain.Service.Generic;
using Web.Domain.Service;
using Web.Entity.UnitOfWork;
using Serilog;
using Microsoft.AspNetCore.Authentication;
using Web.Api.Utils;

namespace Web.Api.ServiceConfigurations
{
    public static class ServiceCollectionExtensions
    {
        //services injections
        public static IServiceCollection AddServiceCollection(this IServiceCollection serviceCollection)
        {
            try
            {
                serviceCollection.AddTransient<SeedData>();
                serviceCollection.AddSingleton<IUnitOfWork, MongoUnitOfWork>();
                serviceCollection.AddScoped<IIdentityService, IdentityService>();
                serviceCollection.AddTransient<IAuthService, AuthService>();
                serviceCollection.AddTransient(typeof(IServiceAsync<,>), typeof(GenericServiceAsync<,>));
                serviceCollection.AddTransient(typeof(IAccountService<,>), typeof(AccountService<,>));
                serviceCollection.AddSingleton<ITokenGenerator, TokenGenerator>();
              //  serviceCollection.AddScoped<IJWTAuthManager, JWTAuthManager>();
                serviceCollection.AddSingleton<INotificationService, NotificationService>();
                serviceCollection.AddSingleton<IEmailService, EmailService>();

                return serviceCollection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "error during configuring services");
                throw;
            }
        }
    }
}
