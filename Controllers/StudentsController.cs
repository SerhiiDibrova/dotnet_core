package Controllers;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    [Route("api/students")]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
    {
        if (createStudentDto == null)
        {
            return BadRequest(new { message = "Student data cannot be null." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _studentService.CreateStudentAsync(createStudentDto);
            return CreatedAtAction(nameof(CreateStudent), new { id = createStudentDto.Id }, createStudentDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the student.", details = ex.Message });
        }
    }
}

public class CreateStudentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
}