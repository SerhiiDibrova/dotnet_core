namespace YourNamespace.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [UniqueEmail]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [MinimumAge(18)]
        [NotFutureDate]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [ValidEnrollmentDate]
        public DateTime EnrollmentDate { get; set; }
    }

    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Logic to check for unique email in the database
            return ValidationResult.Success;
        }
    }

    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public MinimumAgeAttribute(int minimumAge)
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

    public class ValidEnrollmentDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime enrollmentDate)
            {
                if (enrollmentDate > DateTime.Today)
                {
                    return new ValidationResult("Enrollment date must be a valid date and cannot be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class NotFutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.Today)
                {
                    return new ValidationResult("Date of birth cannot be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}