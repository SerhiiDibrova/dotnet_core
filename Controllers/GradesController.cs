namespace YourNamespace.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using YourNamespace.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpGet("{studentId}")]
        public ActionResult<IEnumerable<Grade>> GetGradesForStudent(int? studentId)
        {
            if (!studentId.HasValue)
            {
                return BadRequest("studentId cannot be null.");
            }

            if (studentId <= 0)
            {
                return BadRequest("studentId must be a positive integer.");
            }

            try
            {
                var gradesList = _gradeService.GetGradesForStudent(studentId.Value);

                if (gradesList == null || !gradesList.Any())
                {
                    return NotFound("No grades found for the specified student.");
                }

                return Ok(gradesList);
            }
            catch
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}