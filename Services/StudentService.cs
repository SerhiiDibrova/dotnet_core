package Services;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using YourNamespace.Dtos;
using Microsoft.Extensions.Logging;

public class StudentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(AppDbContext context, ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<(bool Success, string Message)> CreateStudentAsync(StudentDto studentDto)
    {
        if (string.IsNullOrWhiteSpace(studentDto.Name) || string.IsNullOrWhiteSpace(studentDto.Email) || studentDto.Age <= 0)
        {
            return (false, "Invalid student data.");
        }

        if (await _context.Students.AnyAsync(s => s.Email == studentDto.Email))
        {
            return (false, "Email already exists.");
        }

        var student = new Student
        {
            Name = studentDto.Name,
            Email = studentDto.Email,
            Age = studentDto.Age
        };

        try
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return (true, "Student created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a student.");
            return (false, "An unexpected error occurred.");
        }
    }
}