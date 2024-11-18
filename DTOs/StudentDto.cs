namespace YourProject.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StudentDto
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [UniqueEmail]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(StudentDto), nameof(ValidateDate))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enrollment date is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(StudentDto), nameof(ValidateEnrollmentDate))]
        public DateTime EnrollmentDate { get; set; }

        public static ValidationResult ValidateDate(DateTime date, ValidationContext context)
        {
            if (date == default)
            {
                return new ValidationResult("Date must be a valid date.");
            }
            if (date > DateTime.Now)
            {
                return new ValidationResult("Date of birth cannot be in the future.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateEnrollmentDate(DateTime enrollmentDate, ValidationContext context)
        {
            if (enrollmentDate == default)
            {
                return new ValidationResult("Enrollment date must be a valid date.");
            }
            if (enrollmentDate < DateTime.Now)
            {
                return new ValidationResult("Enrollment date cannot be in the past.");
            }
            return ValidationResult.Success;
        }
    }

    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Implement unique email check logic here
            return ValidationResult.Success;
        }
    }
}