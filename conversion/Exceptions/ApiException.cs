namespace Conversion.Exceptions
{
    using System;

    public class ApiException : Exception
    {
        public int StatusCode { get; }

        public ApiException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public static ApiException CreateValidationException(string message)
        {
            return new ApiException(400, message);
        }

        public static ApiException CreateDatabaseException(string message)
        {
            return new ApiException(500, message);
        }
    }
}