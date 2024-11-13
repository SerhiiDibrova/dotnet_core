package conversion.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class StudentDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    public object ToApiResponse()
    {
        return new
        {
            Student = new
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                DateOfBirth = this.DateOfBirth
            },
            Grades = new List<object>()
        };
    }
}

public class GradeDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Subject { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal Score { get; set; }

    [Required]
    public string Term { get; set; }
}