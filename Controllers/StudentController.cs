using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly YourDbContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(YourDbContext context, ILogger<StudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentWithGradesAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("The id parameter is required and must be a valid integer.");
            }

            try
            {
                var student = await _context.Students.Include(s => s.Grades).FirstOrDefaultAsync(s => s.Id == id);

                if (student == null)
                {
                    return NotFound($"No student exists with the specified id: {id}.");
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the student with id {Id}.", id);
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}