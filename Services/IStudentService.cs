package Services;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using YourNamespace.Data;
using YourNamespace.Models;
using Microsoft.Extensions.Logging;

public interface IStudentService
{
    Task<Student> CreateStudentAsync(CreateStudentDto studentDto);
}

public class StudentService : IStudentService
{
    private readonly YourDbContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(YourDbContext context, ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Student> CreateStudentAsync(CreateStudentDto studentDto)
    {
        if (studentDto == null)
        {
            _logger.LogError("Student data cannot be null.");
            throw new ValidationException("Student data cannot be null.");
        }

        var student = new Student
        {
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
            Email = studentDto.Email,
            DateOfBirth = studentDto.DateOfBirth
        };

        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(student);
        if (!Validator.TryValidateObject(student, validationContext, validationResults, true))
        {
            _logger.LogError("Student data is not valid: {ValidationResults}", validationResults);
            throw new ValidationException("Student data is not valid.");
        }

        try
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "An error occurred while saving the student record.");
            throw new ValidationException("An error occurred while saving the student record.", dbEx);
        }

        return student;
    }
}