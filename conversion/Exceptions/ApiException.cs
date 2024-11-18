namespace Conversion.Exceptions
{
    using System;
    using Microsoft.AspNetCore.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public string ErrorMessage { get; }

        public ApiException(int statusCode, string message) : base(message)
        {
            if (statusCode < 400 || statusCode > 599)
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), "Status code must be between 400 and 599.");
            }
            StatusCode = statusCode;
            ErrorMessage = message ?? throw new ArgumentNullException(nameof(message), "Message cannot be null.");
        }

        public static async Task HandleException(HttpContext context, Exception exception, ILogger logger)
        {
            ApiException apiException;

            switch (exception)
            {
                case ArgumentNullException:
                    apiException = new ApiException(StatusCodes.Status400BadRequest, "A required parameter was missing.");
                    break;
                case InvalidOperationException:
                    apiException = new ApiException(StatusCodes.Status409Conflict, "The operation is not valid.");
                    break;
                default:
                    apiException = new ApiException(StatusCodes.Status500InternalServerError, "An unexpected error occurred. Please try again later.");
                    break;
            }

            logger.LogError(exception, "An error occurred: {Message}", exception.Message);
            context.Response.StatusCode = apiException.StatusCode;
            context.Response.ContentType = "application/json";
            var response = JsonSerializer.Serialize(new { error = apiException.ErrorMessage });
            await context.Response.WriteAsync(response);
        }
    }
}