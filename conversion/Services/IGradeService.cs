namespace Conversion.Services
{
    using System.Threading.Tasks;
    using Conversion.Models;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost]
        public async Task<ActionResult<Grade>> AddGradeAsync([FromBody] Grade grade)
        {
            if (grade == null)
            {
                return BadRequest("Grade cannot be null.");
            }

            var addedGrade = await _gradeService.AddGradeAsync(grade);
            return CreatedAtAction(nameof(AddGradeAsync), new { id = addedGrade.Id }, addedGrade);
        }
    }
}