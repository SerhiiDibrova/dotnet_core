package Services;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public interface IStudentService
{
    Task<StudentWithGradesDto> GetStudentWithGradesAsync(int id);
}

public class StudentService : IStudentService
{
    private readonly DbContext _context;

    public StudentService(DbContext context)
    {
        _context = context;
    }

    public async Task<StudentWithGradesDto> GetStudentWithGradesAsync(int id)
    {
        if (id <= 0)
        {
            throw new BadHttpRequestException("The id parameter is required and must be a valid integer.");
        }

        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            throw new KeyNotFoundException($"No student exists with the specified id: {id}");
        }

        var gradesList = await _context.Grades.Where(g => g.StudentId == id).ToListAsync();
        return new StudentWithGradesDto
        {
            Student = student,
            Grades = gradesList
        };
    }
}

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentWithGrades(int id)
    {
        try
        {
            var result = await _studentService.GetStudentWithGradesAsync(id);
            return Ok(result);
        }
        catch (BadHttpRequestException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}

public class StudentWithGradesDto
{
    public Student Student { get; set; }
    public List<Grade> Grades { get; set; }
}

public class Student
{
    // Student properties
}

public class Grade
{
    public int StudentId { get; set; }
    // Other grade properties
}