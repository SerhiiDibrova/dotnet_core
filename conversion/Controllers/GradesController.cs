namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using YourNamespace.Models; 
    using YourNamespace.Services; 
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("api/grades")]
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
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { Errors = errors });
            }

            try
            {
                var addedGrade = await _gradeService.AddGradeAsync(grade);
                if (addedGrade == null)
                {
                    return BadRequest(new { Message = "Failed to add the grade." });
                }
                return Ok(addedGrade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while adding a grade.");
                return StatusCode(500, new { Message = "An unexpected error occurred." });
            }
        }
    }
}