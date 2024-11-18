namespace YourNamespace.Services

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using Microsoft.AspNetCore.Mvc;

public class GradeService : IGradeService
{
    private readonly YourDbContext _context;

    public GradeService(YourDbContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<List<Grade>>> GetGradesForStudentAsync(int studentId)
    {
        var gradesList = await _context.Grades
            .Where(g => g.StudentId == studentId)
            .ToListAsync();

        if (!gradesList.Any())
        {
            return new NotFoundObjectResult("No grades found for the specified student.");
        }

        return gradesList;
    }
}