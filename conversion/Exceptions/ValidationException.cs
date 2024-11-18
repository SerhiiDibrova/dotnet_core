namespace Conversion.Exceptions
{
    using System;
    using System.Text.RegularExpressions;

    public class ValidationException : Exception
    {
        public string ErrorMessage { get; }
        public int StatusCode { get; }

        public ValidationException(string errorMessage, int statusCode) : base(errorMessage)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public ValidationException(string errorMessage) : this(errorMessage, 400)
        {
        }

        public static (bool IsValid, string ErrorMessage, int StatusCode) Validate(string email, string fieldName, int age, Func<string, bool> emailExists)
        {
            try
            {
                ValidateNotNullOrEmpty(email, fieldName);
                ValidateEmailFormat(email);
                ValidateUniqueEmail(email, emailExists);
                ValidateAge(age);
                return (true, null, 0);
            }
            catch (ValidationException ex)
            {
                return (false, ex.ErrorMessage, ex.StatusCode);
            }
        }

        private static void ValidateNotNullOrEmpty(string value, string fieldName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ValidationException($"{fieldName} cannot be null or empty.", 400);
            }
        }

        private static void ValidateEmailFormat(string email)
        {
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ValidationException("Invalid email format.", 400);
            }
        }

        private static void ValidateUniqueEmail(string email, Func<string, bool> emailExists)
        {
            if (emailExists(email))
            {
                throw new ValidationException("Email must be unique.", 400);
            }
        }

        private static void ValidateAge(int age)
        {
            if (age < 0 || age > 120)
            {
                throw new ValidationException("Age must be between 0 and 120.", 400);
            }
        }
    }
}