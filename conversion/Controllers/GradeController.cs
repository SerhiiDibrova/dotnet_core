package conversion.Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using conversion.Services;
using conversion.Dtos;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _gradeService;
    private readonly ILogger<GradeController> _logger;

    public GradeController(IGradeService gradeService, ILogger<GradeController> logger)
    {
        _gradeService = gradeService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> AddGrade([FromBody] GradeDto gradeDto)
    {
        if (gradeDto == null)
        {
            return BadRequest("Grade data is required.");
        }

        if (string.IsNullOrEmpty(gradeDto.StudentId))
        {
            return BadRequest("Student ID is required.");
        }

        if (string.IsNullOrEmpty(gradeDto.CourseId))
        {
            return BadRequest("Course ID is required.");
        }

        if (!decimal.TryParse(gradeDto.Value.ToString(), out var value) || value < 0 || value > 100)
        {
            return BadRequest("Value must be a numeric type between 0 and 100.");
        }

        if (!DateTime.TryParse(gradeDto.DateAssigned, out _))
        {
            return BadRequest("DateAssigned must be in a valid date format.");
        }

        var studentExists = await _gradeService.StudentExists(gradeDto.StudentId);
        var courseExists = await _gradeService.CourseExists(gradeDto.CourseId);

        if (!studentExists)
        {
            return BadRequest("Student ID does not exist.");
        }

        if (!courseExists)
        {
            return BadRequest("Course ID does not exist.");
        }

        try
        {
            var addedGrade = await _gradeService.AddGrade(gradeDto);
            return Ok(new { message = "Grade added successfully.", grade = addedGrade });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while adding the grade.");
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}