package Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.DTOs;
using YourNamespace.Exceptions;
using YourNamespace.Models;

public class StudentService : IStudentService
{
    private readonly YourDbContext _context;

    public StudentService(YourDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> CreateStudentAsync(CreateStudentDto studentDto)
    {
        if (studentDto == null)
        {
            throw new ValidationException("Student data is required.");
        }

        var validationErrors = ValidateStudentDto(studentDto);
        if (validationErrors.Any())
        {
            throw new ValidationException(string.Join(", ", validationErrors));
        }

        if (await _context.Students.AnyAsync(s => s.Email == studentDto.Email))
        {
            throw new ValidationException("Email address must be unique.");
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
            return new OkResult();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Database update error occurred.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while saving the student.", ex);
        }
    }

    private IEnumerable<string> ValidateStudentDto(CreateStudentDto studentDto)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(studentDto.Name))
        {
            errors.Add("Name is required.");
        }
        if (string.IsNullOrWhiteSpace(studentDto.Email))
        {
            errors.Add("Email is required.");
        }
        if (studentDto.Age <= 0)
        {
            errors.Add("Age must be a positive number.");
        }
        return errors;
    }
}