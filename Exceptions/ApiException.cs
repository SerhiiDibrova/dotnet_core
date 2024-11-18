namespace YourProject.Exceptions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Logging;

    public class ApiException : Exception
    {
        public string ErrorMessage { get; }
        public int StatusCode { get; }
        private readonly ILogger<ApiException> _logger;

        public ApiException(string errorMessage, int statusCode, ILogger<ApiException> logger) : base(errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException("Error message cannot be null or empty.", nameof(errorMessage));
            }

            if (statusCode < 100 || statusCode > 599)
            {
                throw new ArgumentOutOfRangeException(nameof(statusCode), "Status code must be a valid HTTP status code.");
            }

            ErrorMessage = errorMessage;
            StatusCode = statusCode;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            LogError();
        }

        private void LogError()
        {
            _logger.LogError($"Error occurred: {ErrorMessage}, Status Code: {StatusCode}");
        }

        public IDictionary<string, object> ToErrorResponse()
        {
            return new Dictionary<string, object>
            {
                { "error", ErrorMessage },
                { "statusCode", StatusCode }
            };
        }
    }
}