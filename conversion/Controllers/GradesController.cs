namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Conversion.Models;
    using Conversion.Services;
    using Microsoft.Extensions.Logging;
    using System.Linq;

    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly ILogger<GradesController> _logger;

        public GradesController(IGradeService gradeService, ILogger<GradesController> logger)
        {
            _gradeService = gradeService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Validation errors", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                var addedGrade = await _gradeService.AddGradeAsync(grade);
                return Ok(addedGrade);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the grade.");
                return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
            }
        }
    }
}