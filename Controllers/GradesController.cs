using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourNamespace.Services;
using YourNamespace.Models;
using System.ComponentModel.DataAnnotations;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] GradeDto gradeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _gradeService.AddGradeAsync(gradeDto);
                if (result == null)
                {
                    return NotFound();
                }

                return CreatedAtAction(nameof(AddGrade), new { id = result.Id }, result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ServiceException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}