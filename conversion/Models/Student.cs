package conversion.Models;

using System;
using System.Collections.Generic;
using System.Linq;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime EnrollmentDate { get; set; }

    private readonly YourDbContext _context;

    public Student(YourDbContext context)
    {
        _context = context;
    }

    public List<Student> GetStudents()
    {
        return _context.Students.ToList();
    }
}