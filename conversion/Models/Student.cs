namespace Conversion.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(50, ErrorMessage = "Email cannot be longer than 50 characters.")]
        public string Email { get; set; }

        public ICollection<Grade> Grades { get; set; }

        public Student()
        {
            Grades = new List<Grade>();
        }
    }

    public class Grade
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [StringLength(50, ErrorMessage = "Subject cannot be longer than 50 characters.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Letter grade is required.")]
        [StringLength(2, ErrorMessage = "Letter grade cannot be longer than 2 characters.")]
        public string Letter { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}