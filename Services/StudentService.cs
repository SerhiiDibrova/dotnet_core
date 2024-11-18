namespace YourNamespace.Services

using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YourNamespace.Data;
using YourNamespace.Models;
using YourNamespace.Dtos;

public class StudentService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(ApplicationDbContext context, ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Student>> CreateStudentAsync(CreateStudentDto studentDto)
    {
        if (studentDto == null)
        {
            return Result<Student>.Failure("Student data cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(studentDto.Name))
        {
            return Result<Student>.Failure("Name cannot be empty.");
        }

        if (!IsValidEmail(studentDto.Email))
        {
            return Result<Student>.Failure("Invalid email format.");
        }

        if (await _context.Students.AnyAsync(s => s.Email == studentDto.Email))
        {
            return Result<Student>.Failure("Email must be unique.");
        }

        if (studentDto.Age < 18 || studentDto.Age > 100)
        {
            return Result<Student>.Failure("Age must be between 18 and 100.");
        }

        var student = new Student
        {
            Name = studentDto.Name,
            Email = studentDto.Email,
            Age = studentDto.Age
        };

        try
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving the student.");
            return Result<Student>.Failure("An error occurred while saving the student. Please try again later.");
        }

        return Result<Student>.Success(student);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var mail = new MailAddress(email);
            return mail.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

public class Result<T>
{
    public T Data { get; private set; }
    public string Error { get; private set; }
    public bool IsSuccess => Error == null;

    private Result(T data, string error)
    {
        Data = data;
        Error = error;
    }

    public static Result<T> Success(T data) => new Result<T>(data, null);
    public static Result<T> Failure(string error) => new Result<T>(default, error);
}