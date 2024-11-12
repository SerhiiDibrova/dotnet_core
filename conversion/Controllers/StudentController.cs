using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Conversion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentWithGrades(int id)
        {
            try
            {
                var student = await _studentService.GetStudentWithGradesAsync(id);
                if (student == null)
                {
                    return NotFound(new { Error = "Student not found." });
                }
                var response = new
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    Grades = student.Grades
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving student with id {Id}", id);
                return StatusCode(500, new { Error = "An error occurred while processing your request." });
            }
        }
    }
}