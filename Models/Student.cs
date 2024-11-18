namespace YourNamespace.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(StudentValidator), nameof(StudentValidator.ValidateDateNotInFuture))]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(StudentValidator), nameof(StudentValidator.ValidateDateNotInFuture))]
        public DateTime EnrollmentDate { get; set; }
    }

    public static class StudentValidator
    {
        public static ValidationResult ValidateDateNotInFuture(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult("Date cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}