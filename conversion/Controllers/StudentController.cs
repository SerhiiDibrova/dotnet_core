using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Conversion.Controllers
{
    public class StudentController : ApiController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [Route("api/students")]
        public async Task<IHttpActionResult> GetAllStudents()
        {
            try
            {
                var studentsList = await _studentService.GetAllStudentsAsync();
                return Ok(studentsList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}