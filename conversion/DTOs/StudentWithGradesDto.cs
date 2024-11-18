namespace YourProjectNamespace.Conversion.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class StudentWithGradesDto
    {
        [Required]
        public StudentDto Student { get; set; } = new StudentDto();

        [Required]
        [MinLength(1)]
        public List<GradeDto> Grades { get; set; } = new List<GradeDto>();
    }

    public class StudentDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }
    }

    public class GradeDto
    {
        [Required]
        [StringLength(100)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        public string Grade { get; set; } = string.Empty;
    }
}