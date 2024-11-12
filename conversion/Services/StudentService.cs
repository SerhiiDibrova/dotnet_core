package conversion.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

    public async Task AddStudentAsync(CreateStudentDto studentDto)
    {
        if (studentDto == null) throw new ArgumentNullException(nameof(studentDto));

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(studentDto);
        if (!Validator.TryValidateObject(studentDto, validationContext, validationResults, true))
        {
            throw new ValidationException("Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage)));
        }

        var existingStudent = await _context.Students
            .FirstOrDefaultAsync(s => s.Email == studentDto.Email);
        if (existingStudent != null) 
        {
            throw new InvalidOperationException("Student with the same email already exists.");
        }

        var student = new Student
        {
            Name = studentDto.Name,
            Age = studentDto.Age,
            Email = studentDto.Email
        };

        try
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while saving the student.");
            throw new InvalidOperationException("An error occurred while saving the student.", ex);
        }
    }
}