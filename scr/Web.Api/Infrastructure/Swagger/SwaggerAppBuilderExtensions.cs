namespace Web.Api.ServiceCollectionsConfigurations.Swagger
{
    public static class SwaggerAppBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerWithUI(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API V1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Web API V2");
                c.DisplayOperationId();
                c.DisplayRequestDuration();
            });
            return app;
        }

    }
}
