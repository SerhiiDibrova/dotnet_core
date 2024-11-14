package DTOs;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class StudentDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [StringLength(200)]
    public string Address { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    public DateTime EnrollmentDate { get; set; }
}

public class GradeDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Subject { get; set; }

    [Range(0, 100)]
    public decimal Score { get; set; }

    [Required]
    [StringLength(50)]
    public string Term { get; set; }
}

public class StudentWithGradesDto
{
    public StudentDto Student { get; set; }
    public List<GradeDto> Grades { get; set; }

    public StudentWithGradesDto(StudentDto student, List<GradeDto> grades)
    {
        Student = student;
        Grades = grades;
    }
}