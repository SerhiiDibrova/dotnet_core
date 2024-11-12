package Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

public class AddGradeDto
{
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public string Date { get; set; }
}

public interface IGradeService
{
    AddGradeDto AddGrade(AddGradeDto gradeDto);
}

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
    public IActionResult AddGrade([FromBody] AddGradeDto gradeDto)
    {
        var validationErrors = new List<string>();

        if (gradeDto.StudentId <= 0)
        {
            validationErrors.Add("StudentId must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(gradeDto.Subject))
        {
            validationErrors.Add("Subject must be a non-empty string.");
        }

        if (gradeDto.Score < 0 || gradeDto.Score > 100)
        {
            validationErrors.Add("Score must be between 0 and 100.");
        }

        if (!DateTime.TryParseExact(gradeDto.Date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dateValue) || dateValue >= DateTime.Now)
        {
            validationErrors.Add("Date must be a valid date in the format YYYY-MM-DD and in the past.");
        }

        if (validationErrors.Count > 0)
        {
            return BadRequest(new { Errors = validationErrors });
        }

        try
        {
            var addedGrade = _gradeService.AddGrade(gradeDto);
            return Ok(addedGrade);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while adding the grade.");
        }
    }
}