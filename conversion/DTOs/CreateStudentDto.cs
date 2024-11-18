namespace Conversion.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateStudentDto
    {
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(CreateStudentDto), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enrollment Date is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(CreateStudentDto), nameof(ValidateEnrollmentDate))]
        public DateTime EnrollmentDate { get; set; }

        public static ValidationResult ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
        {
            if (dateOfBirth > DateTime.Now)
            {
                return new ValidationResult("Date of Birth cannot be in the future.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateEnrollmentDate(DateTime enrollmentDate, ValidationContext context)
        {
            var instance = (CreateStudentDto)context.ObjectInstance;
            if (enrollmentDate < instance.DateOfBirth)
            {
                return new ValidationResult("Enrollment Date cannot be earlier than Date of Birth.");
            }
            if (enrollmentDate > DateTime.Now)
            {
                return new ValidationResult("Enrollment Date cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}