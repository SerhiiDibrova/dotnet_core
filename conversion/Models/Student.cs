namespace Conversion.Models
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
        [AgeValidation(18)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [EnrollmentDateValidation]
        public DateTime EnrollmentDate { get; set; }
    }

    public class AgeValidation : ValidationAttribute
    {
        private readonly int _minimumAge;

        public AgeValidation(int minimumAge)
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

    public class EnrollmentDateValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime enrollmentDate)
            {
                var student = (Student)validationContext.ObjectInstance;
                if (enrollmentDate < student.DateOfBirth)
                {
                    return new ValidationResult("Enrollment date cannot be before the date of birth.");
                }
                if (enrollmentDate > DateTime.Today)
                {
                    return new ValidationResult("Enrollment date cannot be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class UniqueEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Implement logic to check for unique email in the database
            return ValidationResult.Success;
        }
    }
}