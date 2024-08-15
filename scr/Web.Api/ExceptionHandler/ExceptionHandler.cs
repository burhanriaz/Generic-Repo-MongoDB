using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Web.Api
{
    /// <summary>
    /// Middleware - error handling
    /// </summary>
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;

        public ExceptionHandler(RequestDelegate _next, IConfiguration _configuration)
        {
            next = _next;
            configuration = _configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
                if (configuration["Exception:ThrowExceptionAfterLog"] == "True")
                {
                    throw ex;
                }
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //get inner if exists
            var innerExceptionMsg = string.Empty;
            if (exception.InnerException != null)
            {
                innerExceptionMsg = exception.InnerException.Message;
            }

            var errorClass = next.Target.GetType();
            var errorMethod = next.Method;
            var controllerName = GetControllerName(context);

            var errorSerialized = JsonSerializer.Serialize(new
            {
                Error = new
                {
                    Message = $" Controller Name: {controllerName} - Class Name: {errorClass} - Method Name: {errorMethod} - OuterException: {exception.Message} {Environment.NewLine} InnerException: {innerExceptionMsg}",
                    ExceptionName = exception.GetType().Name
                }
            });

            await response.WriteAsync(errorSerialized);

            //serilog
            Log.Error($"ERROR FOUND: {errorSerialized}                  Stack trace: {exception.StackTrace}");
        }

        private string GetControllerName(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>() is { ControllerName: var controllerName })
            {
                return controllerName;
            }
            return "UnknownController";
        } 
    }
}