namespace YourNamespace.Services

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;

public class GradeService
{
    private readonly AppDbContext _context;

    public GradeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddGradeAsync(Grade grade)
    {
        if (grade == null)
        {
            return new BadRequestResult();
        }

        if (!IsValidGrade(grade))
        {
            return new BadRequestResult();
        }

        try
        {
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
            return new OkObjectResult(grade);
        }
        catch (Exception ex)
        {
            // Log the exception details here for debugging
            return new StatusCodeResult(500);
        }
    }

    private bool IsValidGrade(Grade grade)
    {
        return !string.IsNullOrEmpty(grade.StudentId) && 
               !string.IsNullOrEmpty(grade.CourseId) && 
               grade.Score >= 0;
    }
}