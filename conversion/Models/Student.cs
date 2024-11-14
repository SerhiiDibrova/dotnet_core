package conversion.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

public class Student {
    public int Id { get; set; }
    public string Name { get; set; }

    public bool IsValidStudentId(int studentId, DbContext context) {
        return context.Set<Student>().Any(s => s.Id == studentId);
    }
}

public class Grade {
    [Required]
    public int StudentId { get; set; }

    [Range(0, 100)]
    public int Value { get; set; }
}