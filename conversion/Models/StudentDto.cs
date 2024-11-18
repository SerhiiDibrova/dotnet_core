namespace Conversion.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class StudentDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(250)]
        public string Address { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public string EnrollmentStatus { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        public string Major { get; set; }

        [Required]
        public string Gender { get; set; }

        public DateTime? GraduationDate { get; set; }
    }
}