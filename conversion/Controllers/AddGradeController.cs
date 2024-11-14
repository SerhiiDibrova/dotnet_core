```csharp
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using conversion.Models;

namespace conversion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddGradeController : ControllerBase
    {
        private readonly IGradeRepository _gradeRepository;

        public AddGradeController(IGradeRepository gradeRepository)
        {
            if (gradeRepository == null)
            {
                throw new ArgumentNullException(nameof(gradeRepository));
            }

            _gradeRepository = gradeRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody] Grade grade)
        {
            if (_gradeRepository == null)
            {
                return NotFound(new { ErrorMessage = "Grade repository not found" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { ErrorMessage = "Invalid model state", ModelState = ModelState });
            }

            try
            {
                var addedGrade = await _gradeRepository.AddGradeAsync(grade);
                return Ok(addedGrade);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { ErrorMessage = $"Failed to add grade due to invalid operation: {ex.Message}", ExceptionMessage = ex.InnerException?.Message });
            }
            catch (NotSupportedException ex)
            {
                return StatusCode(500, new { ErrorMessage = $"Failed to add grade due to unsupported operation: {ex.Message}", ExceptionMessage = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = "Failed to add grade", ExceptionMessage = ex.InnerException?.Message });
            }
        }
    }
}
```