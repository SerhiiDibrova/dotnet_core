package Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public interface IGradeService
{
    Task<IActionResult> GetGradesForStudentAsync(int studentId);
}

public class GradeService : IGradeService
{
    private readonly Dictionary<int, List<string>> studentGrades = new Dictionary<int, List<string>>
    {
        { 1, new List<string> { "A", "B", "C" } },
        { 2, new List<string> { "B", "C", "A" } }
    };

    public async Task<IActionResult> GetGradesForStudentAsync(int studentId)
    {
        if (studentId <= 0)
        {
            return new BadRequestObjectResult("Invalid studentId.");
        }

        if (!studentGrades.ContainsKey(studentId))
        {
            return new NotFoundObjectResult("No grades found for the specified studentId.");
        }

        var grades = studentGrades[studentId];
        return new OkObjectResult(grades);
    }
}