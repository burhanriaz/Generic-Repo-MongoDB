using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Serilog;

namespace WebApi.ServiceConfigurations
{
    public static class ServiceCollectionAuthenticationExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            try
            {
                var Issuer = configuration["Jwt:Issuer"];
                var Audience = configuration["Jwt:Audience"];
                var LifetimeSeconds = configuration["Jwt:LifetimeSeconds"];

                var publicRsa = RSA.Create();
                publicRsa.FromXmlString(configuration.GetSection("Keys:JwtRsaPublicKeyXml").Value);
                var signingKey = new RsaSecurityKey(publicRsa);

                // JWT API authentication service
                serviceCollection.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        IssuerSigningKey = signingKey,
                        ClockSkew = TimeSpan.FromDays(Convert.ToDouble(LifetimeSeconds))
                    };
                    config.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Log.Error(context.Exception, "Authentication failed.");
                            return Task.CompletedTask;
                        }
                    };
                });

                return serviceCollection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during Configure Authentication");
                throw;
            }
        }
    }
}
