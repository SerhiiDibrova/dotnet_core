package conversion.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    public List<Grade> Grades { get; set; } = new List<Grade>();

    public StudentWithGrades GetStudentWithGrades()
    {
        return new StudentWithGrades
        {
            Id = this.Id,
            Name = this.Name,
            Email = this.Email,
            Grades = this.Grades
        };
    }
}

public class Grade
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Subject { get; set; }

    [Range(0, 100)]
    public int Score { get; set; }
}

public class StudentWithGrades
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    public List<Grade> Grades { get; set; }
}