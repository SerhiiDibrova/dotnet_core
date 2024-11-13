package conversion.Services;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data; 
using YourNamespace.Models; 
using YourNamespace.Interfaces; 

public class GradeService : IGradeService
{
    private readonly ApplicationDbContext _context;

    public GradeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddGradeAsync(Grade grade)
    {
        if (grade == null)
        {
            return new BadRequestObjectResult("Grade object cannot be null.");
        }

        if (grade.StudentId == null)
        {
            return new BadRequestObjectResult("StudentId cannot be null.");
        }

        if (grade.CourseId == null)
        {
            return new BadRequestObjectResult("CourseId cannot be null.");
        }

        if (grade.Value == null)
        {
            return new BadRequestObjectResult("Value cannot be null.");
        }

        if (grade.DateAssigned == null)
        {
            return new BadRequestObjectResult("DateAssigned cannot be null.");
        }

        var studentExists = await _context.Students.AnyAsync(s => s.Id == grade.StudentId);
        var courseExists = await _context.Courses.AnyAsync(c => c.Id == grade.CourseId);

        if (!studentExists || !courseExists)
        {
            return new BadRequestObjectResult("Student or Course does not exist.");
        }

        if (!decimal.TryParse(grade.Value.ToString(), out var value) || value < 0 || value > 100)
        {
            return new BadRequestObjectResult("Value must be a number between 0 and 100.");
        }

        if (!DateTime.TryParse(grade.DateAssigned.ToString(), out _))
        {
            return new BadRequestObjectResult("DateAssigned must be a valid date.");
        }

        try
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        return new OkResult();
    }
}