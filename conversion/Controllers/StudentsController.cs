namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using YourNamespace.Services;

    [Route("api/[controller]")]
    [ApiController]
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
            var studentWithGrades = await _studentService.GetStudentWithGradesAsync(id);
            if (studentWithGrades == null)
            {
                return NotFound();
            }
            return Ok(studentWithGrades);
        }
    }
}