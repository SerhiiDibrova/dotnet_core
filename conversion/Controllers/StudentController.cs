package conversion.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentController> _logger;

    public StudentController(IStudentService studentService, ILogger<StudentController> logger)
    {
        _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GetAllStudents()
    {
        try
        {
            var studentsList = await _studentService.GetAllStudentsAsync();
            return Ok(studentsList ?? new object[0]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving student records.");
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }
}