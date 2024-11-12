package Services;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data; 
using YourNamespace.Models; 
using YourNamespace.DTOs; 
using YourNamespace.Interfaces; 
using Microsoft.Extensions.Logging;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(AppDbContext context, ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result> CreateStudentAsync(CreateStudentDto studentDto)
    {
        if (string.IsNullOrWhiteSpace(studentDto.Name))
        {
            return Result.Failure("Name is required.");
        }

        if (studentDto.Age <= 0)
        {
            return Result.Failure("Age is required and must be a positive number.");
        }

        if (studentDto.Age < 18)
        {
            return Result.Failure("Student must be at least 18 years old.");
        }

        var student = new Student
        {
            Name = studentDto.Name,
            Age = studentDto.Age
        };

        try
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return Result.Success(student);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update error while creating student.");
            return Result.Failure("An error occurred while creating the student.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating student.");
            return Result.Failure("An unexpected error occurred.");
        }
    }
}