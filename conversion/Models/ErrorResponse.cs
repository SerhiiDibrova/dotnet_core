namespace Conversion.Models
{
    public class ErrorResponse
    {
        public string Error { get; set; }

        public ErrorResponse(string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                throw new ArgumentException("Error message cannot be null or empty.", nameof(error));
            }
            Error = error;
        }
    }
}

namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Conversion.Models;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{id}/grades")]
        public async Task<IActionResult> GetStudentWithGrades(int id)
        {
            var student = await _studentService.GetStudentWithGradesAsync(id);
            if (student == null)
            {
                return NotFound(new ErrorResponse("Student not found."));
            }
            return Ok(student);
        }
    }
}

namespace Conversion.Services
{
    using System.Threading.Tasks;

    public interface IStudentService
    {
        Task<StudentDto> GetStudentWithGradesAsync(int id);
    }
}

namespace Conversion.Models
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GradeDto> Grades { get; set; }
    }

    public class GradeDto
    {
        public string Subject { get; set; }
        public int Score { get; set; }
    }
}