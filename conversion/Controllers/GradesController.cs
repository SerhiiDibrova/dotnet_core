using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Conversion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly DbContext _context;
        private readonly ILogger<GradesController> _logger;

        public GradesController(DbContext context, ILogger<GradesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest(new { errors = new List<string> { "Grade object cannot be null." } });
            }

            var validationErrors = new List<string>();

            if (string.IsNullOrEmpty(grade.StudentId))
            {
                validationErrors.Add("StudentId is required.");
            }

            if (grade.Score < 0)
            {
                validationErrors.Add("Score must be a non-negative value.");
            }

            if (validationErrors.Count > 0)
            {
                return BadRequest(new { errors = validationErrors });
            }

            try
            {
                await _context.Grades.AddAsync(grade);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Grade added successfully." });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while adding the grade.");
                return StatusCode(500, new { message = "An error occurred while adding the grade." });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }
    }

    public class Grade
    {
        public string StudentId { get; set; }
        public int Score { get; set; }
    }

    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Grade> Grades { get; set; }
    }
}