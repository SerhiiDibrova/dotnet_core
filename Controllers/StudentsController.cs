namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using YourNamespace.Services;
    using YourNamespace.Models;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentWithGrades(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var studentWithGrades = await _studentService.GetStudentWithGradesAsync(id);
            if (studentWithGrades == null)
            {
                return NotFound();
            }

            var grades = await _studentService.GetGradesByStudentIdAsync(id);
            return Ok(new { Student = studentWithGrades, Grades = grades });
        }
    }
}