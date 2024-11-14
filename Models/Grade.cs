namespace YourNamespace.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Grade
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(0, 100)]
        public double Value { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Invalid date format. Use ISO 8601 format (YYYY-MM-DD).")]
        public DateTime DateAssigned { get; set; }
    }
}