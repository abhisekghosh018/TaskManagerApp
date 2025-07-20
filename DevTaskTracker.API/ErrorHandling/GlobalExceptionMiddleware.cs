using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace DevTaskTracker.API.ErrorHandling
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            try
            {
               await _next(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandeled exception occoured");
                await _next(httpContext);
            }

        }


        private Task HandelExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new
            {
                StatusCodes = StatusCodes.Status500InternalServerError,
                Message = exception.Message,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsJsonAsync(response);
        }
    }


   

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
