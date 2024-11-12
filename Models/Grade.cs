package Models;

using System;
using System.ComponentModel.DataAnnotations;

public class Grade
{
    public int? Id { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Subject { get; set; }

    [Required(ErrorMessage = "Score is required.")]
    [Range(0, 100, ErrorMessage = "Score must be between 0 and 100.")]
    public double Score { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Date must be in the format YYYY-MM-DD.")]
    public DateTime Date { get; set; }
}