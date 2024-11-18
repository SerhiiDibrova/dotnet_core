namespace YourNamespace.Services

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using YourNamespace.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(AppDbContext context, ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ActionResult<IEnumerable<Student>>> GetAllStudentsAsync()
    {
        try
        {
            var students = await _context.Students.ToListAsync();
            if (students == null || students.Count == 0)
            {
                return new ActionResult<IEnumerable<Student>>(new List<Student>());
            }
            return new ActionResult<IEnumerable<Student>>(students);
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Database update error occurred while retrieving students.");
            return new StatusCodeResult(500);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving students.");
            return new StatusCodeResult(500);
        }
    }
}