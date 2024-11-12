using System;
using System.ComponentModel.DataAnnotations;

namespace DTOs
{
    public class CreateStudentDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, 120)]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(CreateStudentDto), nameof(ValidateEnrollmentDate))]
        public DateTime EnrollmentDate { get; set; }

        public static ValidationResult ValidateEnrollmentDate(DateTime enrollmentDate, ValidationContext context)
        {
            if (enrollmentDate > DateTime.Now)
            {
                return new ValidationResult("Enrollment date cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}