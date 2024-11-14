package conversion.Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using YourNamespace.Data; // Adjust the namespace according to your project
using YourNamespace.Models; // Adjust the namespace according to your project
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AddGradeController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public AddGradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpPost]
    public async Task<IActionResult> AddGrade([FromBody] Grade grade)
    {
        if (grade == null || 
            grade.StudentId == null || 
            grade.CourseId == null || 
            grade.Value < 0 || 
            grade.Value > 100 || 
            !DateTime.TryParse(grade.DateAssigned.ToString(), out _))
        {
            return BadRequest("Invalid input data: Ensure all fields are present and valid.");
        }

        var studentExists = await _gradeService.StudentExists(grade.StudentId);
        var courseExists = await _gradeService.CourseExists(grade.CourseId);

        if (!studentExists || !courseExists)
        {
            return BadRequest("Invalid input data: Student or Course does not exist.");
        }

        try
        {
            await _gradeService.AddGrade(grade);
            return Ok(new { id = grade.Id, grade.StudentId, grade.CourseId, grade.Value, grade.DateAssigned });
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, $"An error occurred while saving the grade due to a database issue: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred while saving the grade: {ex.Message}");
        }
    }
}