namespace YourProject.DTOs
{
    using System.Collections.Generic;
    using YourProject.Models;

    public class StudentWithGradesDto
    {
        public Student Student { get; set; }
        public List<Grade> Grades { get; set; }
    }
}