package conversion.Services;

import System.Collections.Generic;
import System.Threading.Tasks;
import Microsoft.AspNetCore.Mvc;
import Microsoft.EntityFrameworkCore;
import YourNamespace.Data; // Adjust the namespace according to your project structure
import YourNamespace.Models; // Adjust the namespace according to your project structure

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> GetAllStudentsAsync()
    {
        var studentsList = await _context.Students.ToListAsync();
        return Ok(studentsList);
    }
}