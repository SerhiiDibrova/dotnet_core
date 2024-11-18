namespace Conversion.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }

        public ErrorResponse(string message, int statusCode, string errorCode = null, string detail = null, string instance = null)
        {
            Message = message;
            StatusCode = statusCode;
            ErrorCode = errorCode;
            Detail = detail;
            Instance = instance;
        }

        public static ErrorResponse CreateValidationErrorResponse(string message, string errorCode = null, string detail = null, string instance = null)
        {
            return new ErrorResponse(message, 400, errorCode, detail, instance);
        }

        public static ErrorResponse CreateNotFoundResponse(string message, string errorCode = null, string detail = null, string instance = null)
        {
            return new ErrorResponse(message, 404, errorCode, detail, instance);
        }

        public static ErrorResponse CreateServerErrorResponse(string message, string errorCode = null, string detail = null, string instance = null)
        {
            return new ErrorResponse(message, 500, errorCode, detail, instance);
        }
    }
}