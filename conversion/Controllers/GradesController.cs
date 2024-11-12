using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GradesController> _logger;

        public GradesController(AppDbContext context, ILogger<GradesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetGradesForStudent(int studentId)
        {
            if (studentId <= 0)
            {
                _logger.LogWarning($"Invalid studentId: {studentId}");
                return BadRequest(new { error = "studentId is required." });
            }

            List<Grade> grades;
            try
            {
                grades = await _context.Grades.Where(g => g.StudentId == studentId).ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database access error while retrieving grades for studentId: {studentId}", studentId);
                return StatusCode(500, new { error = "An error occurred while accessing the database." });
            }

            if (!grades.Any())
            {
                _logger.LogInformation($"No grades found for studentId: {studentId}");
                return NotFound(new { error = "No grades found for the specified student." });
            }

            _logger.LogInformation($"Retrieved grades for studentId: {studentId}, Status: 200 OK");
            return Ok(grades);
        }
    }
}