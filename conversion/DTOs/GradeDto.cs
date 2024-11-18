namespace YourProjectNamespace.Conversion.DTOs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class GradeDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public decimal Score { get; set; }

        [Required]
        public DateTime DateAssigned { get; set; }
    }
}