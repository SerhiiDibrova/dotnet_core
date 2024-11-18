namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Conversion.Services;
    using Conversion.Models;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;
        private readonly ILogger<GradesController> _logger;

        public GradesController(IGradeService gradeService, ILogger<GradesController> logger)
        {
            _gradeService = gradeService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] AddGradeRequest request)
        {
            if (request == null || !ModelState.IsValid)
            {
                _logger.LogWarning("Invalid grade data received.");
                return BadRequest("Invalid grade data.");
            }

            try
            {
                var addedGrade = await _gradeService.AddGradeAsync(request);
                if (addedGrade == null)
                {
                    _logger.LogWarning("Grade addition failed due to business rules.");
                    return BadRequest("Grade addition failed due to business rules.");
                }
                return Ok(addedGrade);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the grade.");
                return StatusCode(500, "An error occurred while adding the grade.");
            }
        }
    }
}