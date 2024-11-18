namespace Conversion.DTOs

using System;
using System.ComponentModel.DataAnnotations;

public class CreateStudentRequest
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(CreateStudentRequest), nameof(ValidateDateNotInFuture))]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(CreateStudentRequest), nameof(ValidateDateNotInFuture))]
    public DateTime EnrollmentDate { get; set; }

    public static ValidationResult ValidateDateNotInFuture(DateTime date, ValidationContext context)
    {
        if (date > DateTime.Now)
        {
            return new ValidationResult("The date cannot be in the future.");
        }
        return ValidationResult.Success;
    }
}