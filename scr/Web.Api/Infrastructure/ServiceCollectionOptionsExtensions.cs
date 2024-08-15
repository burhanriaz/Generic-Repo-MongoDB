using Serilog;
using Web.Api.Infrastructure.Configurations;
using Web.Entity.Infrastructure.Options;
using Web.Entity.Setting;

namespace WebApi.ServiceConfigurations
{
    public static class ServiceCollectionOptionsExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            try
            {
                serviceCollection.Configure<MongoSecretsSettings>(configuration.GetSection("Keys"));
                serviceCollection.Configure<SecretsSettings>(configuration.GetSection("Keys"));
                serviceCollection.Configure<SMTPNetworkCredential>(configuration.GetSection("SMTPNetworkCredential"));
                serviceCollection.Configure<JwtSettings>(configuration.GetSection("Jwt"));
                serviceCollection.Configure<MongoSettings>(configuration.GetSection(nameof(MongoSettings)));
                serviceCollection.Configure<UrlPathsSettings>(configuration.GetSection("UrlPaths"));
                serviceCollection.Configure<ClientUrlsSettings>(configuration.GetSection("ClientUrls"));

                return serviceCollection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "error during Configure Options");
                throw;
            }
        }
    }
}
