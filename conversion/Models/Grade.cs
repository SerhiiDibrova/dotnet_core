package conversion.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class Grade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    public int SubjectId { get; set; }

    [Required]
    [Range(0.0, 100.0)]
    public decimal Score { get; set; }

    [Required]
    [RegularExpression(@"^(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{1,3})?Z|(\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{1,3})?([+-]\d{2}:\d{2}|Z)))$", ErrorMessage = "Date must be in ISO 8601 format.")]
    public DateTime Date { get; set; }
}