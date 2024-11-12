package conversion.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [DataType(DataType.Date)]
    public DateTime EnrollmentDate { get; set; }

    public static string GetStudentsAsJson(List<Student> students)
    {
        return JsonSerializer.Serialize(students);
    }
}