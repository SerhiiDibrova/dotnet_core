namespace Conversion.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;
    using FluentValidation.Validation;

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
            catch (ValidationException validationEx)
            {
                await HandleValidationExceptionAsync(context, validationEx);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleValidationExceptionAsync(HttpContext context, ValidationException validationEx)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Validation errors occurred.",
                Errors = validationEx.Errors
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An error occurred while processing your request."
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}