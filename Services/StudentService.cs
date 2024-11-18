```csharp
namespace YourNamespace.Services

using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.DTOs;
using YourNamespace.Models;
using YourNamespace.Validators;

public class StudentService
{
    private readonly ApplicationDbContext _context;
    private readonly CreateStudentValidator _validator;

    public StudentService(ApplicationDbContext context, CreateStudentValidator validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<IActionResult> CreateStudentAsync(CreateStudentDto dto)
    {
        var validationResult = _validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        if (await _context.Students.AnyAsync(s => s.Email == dto.Email))
        {
            return new ConflictObjectResult("Email is already in use.");
        }

        if (CalculateAge(dto.DateOfBirth.Value) < 18)
        {
            return new BadRequestObjectResult("Student must be at least 18 years old.");
        }

        var student = new Student
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth.Value,
            EnrollmentDate = dto.EnrollmentDate.Value
        };

        try
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(500);
        }

        return new OkResult();
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}
```