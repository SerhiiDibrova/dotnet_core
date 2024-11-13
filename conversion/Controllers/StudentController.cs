package conversion.Controllers;

using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using YourNamespace.Services;

public class StudentController : ControllerBase
{
    private readonly StudentService _studentService;

    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task<IHttpActionResult> GetAllStudents()
    {
        try
        {
            var studentsList = await _studentService.GetAllStudentsAsync();
            return Ok(studentsList);
        }
        catch (Exception ex)
        {
            // Log the exception details here
            return Content(HttpStatusCode.InternalServerError, new { message = "An error occurred while retrieving student records." });
        }
    }
}