using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conversion.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }

        public ErrorResponse(string message)
        {
            Message = message;
        }

        public static IActionResult HandleError(ILogger logger, Exception ex)
        {
            logger.LogError(ex, ex.ToString());
            var errorResponse = new ErrorResponse(ex.Message);
            return new ObjectResult(errorResponse)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}