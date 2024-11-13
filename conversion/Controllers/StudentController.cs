package conversion.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using YourNamespace.Models;

public class StudentController : ApiController
{
    private readonly AppDbContext _context;

    public StudentController()
    {
        _context = new AppDbContext();
    }

    [HttpGet]
    public IHttpActionResult GetAllStudents()
    {
        try
        {
            List<Student> studentsList = _context.Students.ToList();
            return Ok(studentsList);
        }
        catch (Exception)
        {
            return InternalServerError(new Exception("An error occurred while retrieving student records."));
        }
    }
}