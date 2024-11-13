package conversion.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.DTOs;
using YourNamespace.Entities;
using YourNamespace.Exceptions;

public class StudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateStudentAsync(CreateStudentDto studentDto)
    {
        if (studentDto == null)
        {
            throw new ApiException("Validation error: Student data cannot be null.");
        }

        var validationErrors = new List<string>();

        if (string.IsNullOrWhiteSpace(studentDto.FirstName))
        {
            validationErrors.Add("First name is required.");
        }
        if (string.IsNullOrWhiteSpace(studentDto.LastName))
        {
            validationErrors.Add("Last name is required.");
        }
        if (string.IsNullOrWhiteSpace(studentDto.Email))
        {
            validationErrors.Add("Email is required.");
        }
        if (string.IsNullOrWhiteSpace(studentDto.DateOfBirth) || !DateTime.TryParse(studentDto.DateOfBirth, out var dateOfBirth) || dateOfBirth > DateTime.Now)
        {
            validationErrors.Add("Date of birth must be a valid date and not in the future.");
        }
        if (string.IsNullOrWhiteSpace(studentDto.EnrollmentDate) || !DateTime.TryParse(studentDto.EnrollmentDate, out var enrollmentDate) || enrollmentDate > DateTime.Now)
        {
            validationErrors.Add("Enrollment date must be a valid date and not in the future.");
        }

        if (validationErrors.Count > 0)
        {
            throw new ApiException("Validation errors: " + string.Join(" ", validationErrors));
        }

        if (await _context.Students.AnyAsync(s => s.Email == studentDto.Email))
        {
            throw new ApiException("Validation error: Email must be unique.");
        }

        var student = new Student
        {
            FirstName = studentDto.FirstName,
            LastName = studentDto.LastName,
            Email = studentDto.Email,
            DateOfBirth = dateOfBirth,
            EnrollmentDate = enrollmentDate
        };

        _context.Students.Add(student);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new ApiException("An error occurred while saving the student record: " + ex.Message, ex);
        }
    }
}