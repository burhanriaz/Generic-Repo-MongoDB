using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Web.Api.Infrastructure.Configurations;
using Web.Domain.Service;
using Web.Entity.Infrastructure.Options;

namespace Web.Api.ServiceConfigurations
{
    public static class ServiceCollectionNamedDependenciesExtensions
    {
        //private static readonly Dictionary<string, object> _namedDependencies = new Dictionary<string, object>();
        public static IServiceCollection AddNamedDependencies(this IServiceCollection serviceCollection, IWebHostEnvironment env, Dictionary<string, object> _namedDependencies)
        {
            try
            {   
                var serviceProvider = serviceCollection.BuildServiceProvider();
                var urlsSettings = serviceProvider.GetService<IOptions<UrlPathsSettings>>().Value;
                var clientUrlsSettings = serviceProvider.GetService<IOptions<ClientUrlsSettings>>().Value;


                _namedDependencies.Add(NotificationService.EmailVerificationLink, clientUrlsSettings.WebAppUrl + urlsSettings.EmailVerificationRedirectUrlPath);

                _namedDependencies.Add(NotificationService.PasswordRestorationRedirectUrl, clientUrlsSettings.WebAppUrl + urlsSettings.PasswordRestorationRedirectUrlPath);

                return serviceCollection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "error during Configure Named Dependencies");
                throw;
            }

        }
    }
}
