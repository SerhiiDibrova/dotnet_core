package conversion.Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using conversion.Services;
using conversion.Dtos;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(createStudentDto);

        if (!Validator.TryValidateObject(createStudentDto, validationContext, validationResults, true))
        {
            var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
            return BadRequest(new { Errors = errors });
        }

        try
        {
            var student = await _studentService.AddStudentAsync(createStudentDto);
            return Ok(student);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
        }
    }
}