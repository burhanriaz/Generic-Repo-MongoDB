using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.Options;
using Web.Entity.Infrastructure.Options;
using Serilog;
using MongoDB.Driver;

namespace Web.Api.ServiceConfigurations
{
    public static class ServiceCollectionHangfireExtensions
    {
        // Helping link for hangfire 
        //https://davek.dev/using-hangfire-and-mongodb-for-scheduling-in-net-6#heading-set-up-hangfire
        public static string hangFirePrefix = "Hangfire";
        public static IServiceCollection AddHangfire(this IServiceCollection serviceCollection, IWebHostEnvironment webHostEnvironment, IConfiguration _configuration)
        {
            try
            {

                if (webHostEnvironment.IsDevelopment())
                {
                    hangFirePrefix = "Hangfire_Local";
                }
                else
                {
                    hangFirePrefix = "Hangfire_V1";
                }
                serviceCollection.AddHangfire((sp, configuration) =>
                {
                    var mongoSecretsSettings = sp.GetService<IOptions<MongoSecretsSettings>>().Value;
                    var dbSettings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;

                    dbSettings = _configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();
                    // BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
                
                    var mongoUrlBuilder = new MongoUrlBuilder(mongoSecretsSettings.MongoConnectionString + dbSettings.DatabaseName);
                    mongoUrlBuilder.AuthenticationSource = "admin";
                    configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseMongoStorage($"{mongoUrlBuilder}", new MongoStorageOptions
                        {
                            MigrationOptions = new MongoMigrationOptions
                            {
                                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                                BackupStrategy = new CollectionMongoBackupStrategy()
                            },
                            //MigrationOptions = new MongoMigrationOptions(MongoMigrationStrategy.Migrate),
                            Prefix = hangFirePrefix,
                            InvisibilityTimeout = TimeSpan.FromMinutes(5),
                            CheckConnection = false
                        });
                });

                serviceCollection.AddHangfireServer();

                // how jobs added here
                // serviceCollection.AddTransient<IPaymentCancellationJob, PaymentCancellationJob>();

                return serviceCollection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "error during Configure Hangfire");
                throw;
            }
        }
    }
}
