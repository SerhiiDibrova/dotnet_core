namespace YourNamespace.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Grade
    {
        public int? Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public decimal Value { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}Z$", ErrorMessage = "DateAssigned must be in a valid ISO 8601 format.")]
        public DateTime DateAssigned { get; set; }
    }
}