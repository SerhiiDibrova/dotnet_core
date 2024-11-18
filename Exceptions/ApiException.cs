namespace YourProject.Exceptions
{
    using System;

    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public static ApiException ValidationError(string message)
        {
            return new ApiException(400, message);
        }

        public static ApiException InternalServerError(string message)
        {
            return new ApiException(500, message);
        }
    }
}