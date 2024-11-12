package Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YourNamespace.Services;
using YourNamespace.Models;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddGrade([FromBody] AddGradeRequest request)
    {
        if (request == null || !ModelState.IsValid)
        {
            var errors = ModelState.Where(e => e.Value.Errors.Count > 0)
                                   .Select(e => new { Field = e.Key, Errors = e.Value.Errors.Select(err => err.ErrorMessage) })
                                   .ToList();
            return BadRequest(new { Message = "Invalid request", Errors = errors });
        }

        try
        {
            var grade = await _gradeService.AddGradeAsync(request);
            return Ok(new { Grade = grade });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
        }
    }
}