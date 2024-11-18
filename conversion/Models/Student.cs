namespace Conversion.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [UniqueEmail]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [AgeValidation(18)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [EnrollmentDateValidation]
        public DateTime EnrollmentDate { get; set; }
    }

    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            var context = (YourDbContext)validationContext.GetService(typeof(YourDbContext));
            if (context.Students.Any(s => s.Email == email))
            {
                return new ValidationResult("Email must be unique.");
            }
            return ValidationResult.Success;
        }
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
                    return new ValidationResult($"Student must be at least {_minimumAge} years old.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class EnrollmentDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime enrollmentDate)
            {
                if (enrollmentDate > DateTime.Today)
                {
                    return new ValidationResult("Enrollment date cannot be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}