namespace YourNamespace.Services

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;

public class StudentService
{
    private readonly YourDbContext _context;

    public StudentService(YourDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddStudentAsync(Student student)
    {
        if (student == null)
        {
            return new BadRequestObjectResult("Student object is null.");
        }

        var validationErrors = ValidateStudent(student);
        if (validationErrors.Any())
        {
            return new BadRequestObjectResult(validationErrors);
        }

        if (await _context.Students.AnyAsync(s => s.Email == student.Email))
        {
            return new ConflictObjectResult("Email already exists.");
        }

        try
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        catch (DbUpdateException ex)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    private IEnumerable<string> ValidateStudent(Student student)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(student.Name))
        {
            errors.Add("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(student.Email))
        {
            errors.Add("Email is required.");
        }

        if (student.DateOfBirth >= DateTime.Now)
        {
            errors.Add("Date of Birth must be in the past.");
        }

        if (student.EnrollmentDate < DateTime.Now)
        {
            errors.Add("Enrollment Date must be today or in the future.");
        }

        return errors;
    }
}