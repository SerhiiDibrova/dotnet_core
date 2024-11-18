namespace Conversion.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Grade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(0, 100)]
        public decimal Score { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Grade), nameof(ValidateDateAssigned))]
        public DateTime DateAssigned { get; set; }

        public static ValidationResult ValidateDateAssigned(DateTime dateAssigned, ValidationContext context)
        {
            if (dateAssigned > DateTime.Now)
            {
                return new ValidationResult("DateAssigned cannot be in the future.");
            }
            return ValidationResult.Success;
        }
    }
}