namespace Conversion.Services

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Conversion.Data;
using Conversion.Models;

public class StudentService : IStudentService
{
    private readonly ApplicationDbContext _context;
    private readonly IValidator<CreateStudentRequest> _validator;

    public StudentService(ApplicationDbContext context, IValidator<CreateStudentRequest> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Result> CreateStudentAsync(CreateStudentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Result.Failure("Name cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return Result.Failure("Email cannot be null or empty.");
        }

        if (!IsValidEmail(request.Email))
        {
            return Result.Failure("Email format is invalid.");
        }

        if (request.Age < 18)
        {
            return Result.Failure("Student must be at least 18 years old.");
        }

        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Result.Failure(validationResult.Errors);
        }

        if (await _context.Students.AnyAsync(s => s.Email == request.Email))
        {
            return Result.Failure("Email must be unique.");
        }

        var student = new Student
        {
            Name = request.Name,
            Email = request.Email,
            Age = request.Age
        };

        try
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return Result.Success(student);
        }
        catch (DbUpdateException)
        {
            return Result.Failure("An error occurred while saving the student.");
        }
        catch (Exception ex)
        {
            return Result.Failure($"An unexpected error occurred: {ex.Message}");
        }
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }
}