package DTOs;

using System;
using System.ComponentModel.DataAnnotations;

public class AddGradeRequest
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Subject { get; set; }

    [Required]
    [Range(0, 100)]
    public int Score { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; }
}