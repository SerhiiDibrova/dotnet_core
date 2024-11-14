namespace YourNamespace.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        public DateTime DateOfBirth { get; set; }

        public ICollection<Grade> Grades { get; set; }

        public Student()
        {
            Grades = new List<Grade>();
        }
    }

    public class Grade
    {
        [Key]
        public int GradeId { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Letter is required.")]
        [StringLength(2, ErrorMessage = "Letter cannot be longer than 2 characters.")]
        public string Letter { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}