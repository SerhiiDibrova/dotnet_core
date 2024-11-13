package conversion.Models;

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
    [Range(0, 100)]
    public decimal Value { get; set; }

    [Required]
    public DateTime DateAssigned { get; set; }
}