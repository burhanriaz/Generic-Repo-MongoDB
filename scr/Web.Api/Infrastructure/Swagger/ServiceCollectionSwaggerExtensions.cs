using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Web.Api.ServiceCollectionsConfigurations.Swagger;

namespace Web.Api.ServiceCollectionsConfigurations.Swagger
{
    public static class ServiceCollectionSwaggerExtensions
    {
        public static IServiceCollection AddSwaggerGen(this IServiceCollection serviceCollection)
        {
            try
            {

                serviceCollection.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web API", Version = "v1" });
                    c.SwaggerDoc("v2", new OpenApiInfo { Title = "Web API", Version = "v2" });


                    var security = new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        //BearerFormat = "JWT",
                    };
                    c.AddSecurityDefinition("Bearer", security);

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                           new List<string>(){ }
                        }
                    };
                    c.AddSecurityRequirement(securityRequirement);

                    //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    //c.IncludeXmlComments(xmlPath);
                });

                return serviceCollection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "error during Swagger Configure");
                throw;
            }
        }
    }
}

