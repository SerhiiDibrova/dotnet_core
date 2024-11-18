namespace YourNamespace.Services

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;

public class GradeService : IGradeService
{
    private readonly ApplicationDbContext _context;

    public GradeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IResult> GetStudentWithGradesAsync(int id)
    {
        var student = await _context.Students
            .Include(s => s.Grades)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
        {
            return Results.NotFound();
        }

        var studentWithGrades = new StudentWithGradesDto
        {
            Id = student.Id,
            Name = student.Name,
            Grades = student.Grades.Select(g => new GradeDto
            {
                Id = g.Id,
                Subject = g.Subject,
                Score = g.Score
            }).ToList()
        };

        return Results.Ok(studentWithGrades);
    }
}