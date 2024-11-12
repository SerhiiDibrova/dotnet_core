package conversion.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class GradeService : IGradeService
{
    private readonly AppDbContext _context;

    public GradeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> GetGradesAsync(int? studentId)
    {
        if (!studentId.HasValue)
        {
            return new BadRequestObjectResult("studentId is required");
        }

        if (studentId <= 0)
        {
            return new BadRequestObjectResult("studentId must be a valid positive integer");
        }

        try
        {
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
        catch (Exception ex)
        {
            // Log the exception (logging mechanism not shown here)
            return new StatusCodeResult(500);
        }
    }
}