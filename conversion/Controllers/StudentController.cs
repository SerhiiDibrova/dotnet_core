namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;

    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult<List<Student>> GetAllStudents()
        {
            try
            {
                var studentList = _context.Students.ToList();
                if (!studentList.Any())
                {
                    return Ok(new List<Student>());
                }
                return Ok(studentList);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving students." });
            }
        }
    }
}