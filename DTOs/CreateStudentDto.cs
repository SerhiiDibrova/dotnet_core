namespace YourNamespace.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateStudentDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [UniqueEmail]
        public string Email { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [AgeValidation(18)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Enrollment date is required.")]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }
    }

    public class AgeValidationAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public AgeValidationAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var age = DateTime.Today.Year - dateOfBirth.Year;
                if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;
                if (age < _minimumAge)
                {
                    return new ValidationResult($"The student must be at least {_minimumAge} years old.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Logic to check for unique email in the database should be implemented here.
            return ValidationResult.Success;
        }
    }
}