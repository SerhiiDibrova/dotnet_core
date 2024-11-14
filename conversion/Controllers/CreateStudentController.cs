```csharp
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using conversion.Models;
using conversion.Services;
using Microsoft.Extensions.Logging;

namespace conversion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreateStudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<CreateStudentController> _logger;

        public CreateStudentController(IStudentService studentService, ILogger<CreateStudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate required fields
            if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName) || string.IsNullOrWhiteSpace(model.Email))
            {
                _logger.LogError("Invalid student data: missing required fields.");
                return BadRequest("Missing required fields");
            }

            try
            {
                var existingStudent = await _studentService.GetStudentAsync(model.Email);

                if (existingStudent != null)
                {
                    _logger.LogWarning($"Duplicate student found with email {model.Email}.");
                    return Conflict("A student with the same email already exists.");
                }

                var studentId = await _studentService.CreateStudentAsync(model);

                if (studentId <= 0)
                {
                    _logger.LogError($"Invalid student ID returned from CreateStudentAsync method for student with email {model.Email}.");
                    return StatusCode(500, "Internal Server Error");
                }

                _logger.LogInformation($"New student created with ID {studentId} and email {model.Email}.");

                return CreatedAtAction(nameof(CreateStudent), new { id = studentId }, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while creating a new student with email {model.Email}: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
```