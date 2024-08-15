using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using WebApi.ServiceConfigurations;
using Web.Api.ServiceConfigurations;
using Web.Entity.Infrastructure;
using Serilog;
using Web.Api;
using Hangfire;
using Microsoft.Extensions.Options;
using Web.Entity.Infrastructure.Options;
using Web.Api.ServiceCollectionsConfigurations.Authorization;
using Web.Api.ServiceCollectionsConfigurations.Swagger;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Hosting;

Log.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);
var HostingEnvironment  = builder.Environment;
// Configure app configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
Dictionary<string, object> _namedDependencies = new Dictionary<string, object>();

builder.WebHost.UseUrls("https://techgenieatlas.com:5000"); // Specify the URL and port

#region CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyDefaultPolicy", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();


    });
});

Log.Information("Add Cors ");
#endregion
builder.Services.AddControllers();
// add ConfigureOptions
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddDatabaseIdentity(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddPermissionAuthorization();
//builder.Services.AddAuthorization(options =>
//{
//    options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

//});
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
Log.Information("Add Controllers");

builder.Services.AddSwaggerGen();
// Add services to the container.
//builder.Configuration.AddUserSecrets<Program>();
//builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Register Mongo serialization maps
MongoMapRegistrator.RegisterMaps();

builder.Services.AddValidators();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new QueryStringApiVersionReader("version");
});

builder.Services.AddAutoMapper();
builder.Services.AddHangfire(HostingEnvironment, builder.Configuration);
builder.Services.AddNamedDependencies(HostingEnvironment, _namedDependencies);
builder.Services.AddServiceTypeResolvers(_namedDependencies);

//services injections
builder.Services.AddServiceCollection();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();
//Start webHost
try
{
    Log.Information("Starting web host");
    // Configure the HTTP request pipeline.

    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var seedData = services.GetRequiredService<SeedData>();

            seedData.Seed();
        }
        var options = app.Services.GetService<IOptionsMonitor<SecretsSettings>>();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHangfireDashboard();
            //app.UseMiddleware<ExceptionHandler>();
        }
        else
        {
            app.UseMiddleware<ExceptionHandler>();
        }

        app.UseSerilogRequestLogging();
        //app.UseHttpsRedirection(); // should be enabled if nginx/apache doesn't handles http to https
        app.UseRouting();

        app.UseAuthentication();
        app.UseRequestMiddleware();
        app.UseAuthorization();
        //app.UseCors("CorsPolicy-public");  //apply to every request
        app.UseCors("MyDefaultPolicy");

        // Swagger API documentation
        app.UseSwaggerWithUI();

        app.UseRequestLocalization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
       
        app.Run();
    }
    catch (Exception ex)
    {
        Log.Error($"Exception in configure services: {ex.Message}");
        Console.WriteLine($"Exception in configure services: {ex.Message}");
    }

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}



//static void ConfigureOptions(WebApplicationBuilder builder)
//{
//    builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));

//}

