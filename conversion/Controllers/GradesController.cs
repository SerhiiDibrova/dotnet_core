package conversion.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GradesController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradesController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpGet("api/grades/{studentId}")]
    public async Task<IActionResult> GetGradesForStudent(int studentId)
    {
        if (studentId <= 0)
        {
            return BadRequest("The studentId must be a positive integer.");
        }

        var gradesList = await _gradeService.GetGradesForStudentAsync(studentId);

        if (gradesList == null || gradesList.Count == 0)
        {
            return NotFound("No grades were found for the student with the specified studentId.");
        }

        return Ok(gradesList);
    }
}