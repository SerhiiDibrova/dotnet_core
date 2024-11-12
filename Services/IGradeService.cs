package Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public interface IGradeService
{
    Task<AddGradeResponse> AddGradeAsync(AddGradeRequest request);
}

public class AddGradeRequest
{
    public string StudentId { get; set; }
    public string Subject { get; set; }
    public int Grade { get; set; }
}

public class AddGradeResponse
{
    public Grade Grade { get; set; }
}

public class Grade
{
    public string StudentId { get; set; }
    public string Subject { get; set; }
    public int GradeValue { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpPost]
    public async Task<IActionResult> AddGrade([FromBody] AddGradeRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.StudentId) || string.IsNullOrEmpty(request.Subject) || request.Grade < 0)
        {
            return BadRequest("Invalid request.");
        }

        var response = await _gradeService.AddGradeAsync(request);
        return Ok(response.Grade);
    }
}

public class GradeService : IGradeService
{
    private readonly List<Grade> grades = new List<Grade>();

    public async Task<AddGradeResponse> AddGradeAsync(AddGradeRequest request)
    {
        try
        {
            var grade = new Grade
            {
                StudentId = request.StudentId,
                Subject = request.Subject,
                GradeValue = request.Grade
            };

            grades.Add(grade);

            return await Task.FromResult(new AddGradeResponse { Grade = grade });
        }
        catch
        {
            throw new Exception("An error occurred while adding the grade.");
        }
    }
}