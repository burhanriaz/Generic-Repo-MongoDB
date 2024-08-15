using AutoMapper;
using Web.Domain.Mapping;
using Serilog;

namespace Web.Api.ServiceConfigurations
{
    public static class ServiceCollectionAutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection)
        {
            try
            {
                serviceCollection.AddAutoMapper(typeof(MappingProfile));
                serviceCollection.AddSingleton(provider => new MapperConfiguration(cfg =>
                {
                    cfg.ConstructServicesUsing(provider.GetService);
                    cfg.AddProfile(new MappingProfile());
                }).CreateMapper());

                return serviceCollection;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "error during Configure AutoMapper");
                throw;
            }
        }


    }
}
