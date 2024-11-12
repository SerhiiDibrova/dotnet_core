package conversion.Services;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;

public class StudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<object> GetStudentWithGradesAsync(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return null;
        }

        var grades = await _context.Grades.Where(g => g.StudentId == id).ToListAsync();

        return new
        {
            Student = student,
            Grades = grades
        };
    }
}