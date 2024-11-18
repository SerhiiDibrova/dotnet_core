namespace Conversion.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(context, ex, "A required argument was null.");
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, ex, "An invalid operation occurred.");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, "An unexpected error occurred.");
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception, string message)
        {
            _logger.LogError(exception, message);

            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}