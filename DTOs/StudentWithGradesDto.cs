namespace YourProject.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class StudentWithGradesDto
    {
        [Required]
        public Student Student { get; set; }

        [Required]
        public List<Grade> Grades { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class Grade
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string LetterGrade { get; set; }
    }
}