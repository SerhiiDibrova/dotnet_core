using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourNamespace.Services;
using YourNamespace.Models;
using Microsoft.Extensions.Logging;

namespace YourNamespace.Controllers
{
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

        [HttpGet]
        public IHttpActionResult getAllStudents()
        {
            try
            {
                var studentsList = await _studentService.GetAllStudentsAsync();
                return Ok(studentsList);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving students.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}