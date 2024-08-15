using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Serilog;
using System.Text.RegularExpressions;
using Web.Entity.Entity.Identity;
using Web.Entity.Infrastructure.Options;

namespace Web.Api.ServiceConfigurations
{
    public static class ServiceCollectionDatabaseIdentityExtensions
    {
        public static IServiceCollection AddDatabaseIdentity(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            try
            {
                var mongoSecretsSettings = serviceCollection.BuildServiceProvider().GetService<IOptions<MongoSecretsSettings>>().Value;
                var dbSettings = serviceCollection.BuildServiceProvider().GetRequiredService<IOptions<MongoSettings>>().Value;

                var mongoDbSettings = configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();

                serviceCollection.AddIdentity<Users, Roles>()
                    .AddMongoDbStores<Users, Roles, Guid>
                    (
                        mongoSecretsSettings.MongoConnectionString,
                        mongoDbSettings.DatabaseName
                    ).AddDefaultTokenProviders();
                serviceCollection.Configure<DataProtectionTokenProviderOptions>(options =>
                {
                    options.TokenLifespan = TimeSpan.FromHours(2);
                });
                //var conventionPack = new ConventionPack
                //{
                //    new IgnoreExtraElementsConvention(true),
                //    new NamedIdMemberConvention("Id", "_id")
                //};

                //ConventionRegistry.Register("CustomConventions", conventionPack, t => t == typeof(Users));

                return serviceCollection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "error during Configure Database Identity");
                throw;
            }
        }
    }
}