namespace Conversion.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class StudentWithGradesDto
    {
        [Required]
        public StudentDto Student { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "The grades collection must contain at least one grade.")]
        public List<GradeDto> Grades { get; set; } = new List<GradeDto>();
    }

    public class StudentDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The Id must be a positive integer.")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }

    public class GradeDto
    {
        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2, ErrorMessage = "The LetterGrade cannot be empty.")]
        public string LetterGrade { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal NumericGrade { get; set; }
    }
}