package conversion.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data; // Adjust the namespace according to your project structure
using YourNamespace.DTOs; // Adjust the namespace according to your project structure

public interface IGradeService
{
    Task<IActionResult> GetGradesForStudentAsync(int studentId);
}

public class GradeService : IGradeService
{
    private readonly AppDbContext _context;

    public GradeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> GetGradesForStudentAsync(int studentId)
    {
        if (studentId <= 0)
        {
            return new BadRequestObjectResult("The studentId is required and must be a valid integer.");
        }

        var grades = await _context.Grades
            .Where(g => g.StudentId == studentId)
            .ToListAsync();

        var gradeDtos = grades.Select(g => new GradeDto
        {
            Id = g.Id,
            StudentId = g.StudentId,
            Subject = g.Subject,
            Score = g.Score
        }).ToList();

        return new OkObjectResult(gradeDtos);
    }
}