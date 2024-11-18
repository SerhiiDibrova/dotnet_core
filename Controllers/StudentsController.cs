namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using YourNamespace.Services;
    using YourNamespace.Dtos;
    using System.Text.RegularExpressions;

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {
            if (createStudentDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (createStudentDto.Age < 18)
            {
                return BadRequest("Student must be at least 18 years old.");
            }

            if (!Regex.IsMatch(createStudentDto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return BadRequest("Invalid email format.");
            }

            try
            {
                var student = await _studentService.CreateStudentAsync(createStudentDto);
                return Ok(student);
            }
            catch (ConflictException)
            {
                return Conflict("Email already exists.");
            }
            catch
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}