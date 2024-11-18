namespace Conversion.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public int? Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime EnrollmentDate { get; set; }
    }
}