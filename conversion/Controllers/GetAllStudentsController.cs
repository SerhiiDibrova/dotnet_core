```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using conversion.Models;
using conversion.Services;
using Microsoft.Extensions.Logging;

namespace conversion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            if (studentService == null)
            {
                throw new ArgumentNullException(nameof(studentService));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _studentService = studentService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all students.
        /// </summary>
        /// <returns>A list of Student objects.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            try
            {
                if (_studentService == null)
                {
                    _logger.LogError("Student service is not initialized.");
                    return StatusCode(500, "Internal Server Error: Student service is not initialized.");
                }

                var students = await _studentService.GetAllStudentsAsync();
                if (students == null)
                {
                    _logger.LogWarning("No students found.");
                    return NotFound("No students found.");
                }

                if (!students.Any())
                {
                    _logger.LogInformation("No content returned for students.");
                    return NoContent();
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all students: {0}", ex.Message);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
```