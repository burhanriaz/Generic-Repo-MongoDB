using FluentValidation;
using Web.Domain.Models.Account;
using Web.Domain.Utils.Validators;
using Web.Domain.Utils.Validators.User;
using Serilog;

namespace Web.Api.ServiceConfigurations
{
    public static class ServiceCollectionValidatorsExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
        {
            try
            {
                serviceCollection.AddSingleton(typeof(IValidator<UserViewModel>), typeof(UserValidator));
                serviceCollection.AddSingleton(typeof(IValidator<RolesViewModel>), typeof(RoleValidator));
                serviceCollection.AddSingleton(typeof(IValidator<LoginViewModel>), typeof(LoginValidator));
                serviceCollection.AddSingleton(typeof(IValidator<EmailConfirmationViewModel>), typeof(EmailConfirmationValidator));
                serviceCollection.AddSingleton(typeof(IValidator<ChangePasswordViewModel>), typeof(ChangePasswordValidator));

                return serviceCollection;
            }
            catch (Exception ex)
            {

                Log.Error(ex, "error during Configure Validators");
                throw;
            }
        }
    }
}
