package DTOs;

using System;
using System.ComponentModel.DataAnnotations;

public class AddGradeDto
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal Score { get; set; }

    [Required]
    public DateTime Date { get; set; }
}