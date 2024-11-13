package conversion.Data;

import Microsoft.EntityFrameworkCore;
import Microsoft.EntityFrameworkCore.Infrastructure;
import Microsoft.EntityFrameworkCore.Storage;
import System;
import System.Collections.Generic;
import System.Linq;
import System.Threading.Tasks;

public class AppDbContext : DbContext
{
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Student> Students { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public async Task<IActionResult> AddStudentAsync(Student student)
    {
        if (student == null || string.IsNullOrEmpty(student.Name))
        {
            return new BadRequestObjectResult("400 Bad Request: Invalid student data.");
        }

        try
        {
            Students.Add(student);
            await SaveChangesAsync();
            return new OkResult();
        }
        catch (DbUpdateException ex)
        {
            return new StatusCodeResult(500);
        }
    }

    public async Task<IActionResult> AddGradeAsync(Grade grade)
    {
        if (grade == null || grade.StudentId <= 0 || grade.Score < 0)
        {
            return new BadRequestObjectResult("400 Bad Request: Invalid grade data.");
        }

        var studentExists = await Students.AnyAsync(s => s.Id == grade.StudentId);
        if (!studentExists)
        {
            return new BadRequestObjectResult("400 Bad Request: Non-existent student ID.");
        }

        try
        {
            Grades.Add(grade);
            await SaveChangesAsync();
            return new OkResult();
        }
        catch (DbUpdateException ex)
        {
            return new StatusCodeResult(500);
        }
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        return await Students.ToListAsync();
    }

    public async Task<List<Grade>> GetGradesAsync()
    {
        return await Grades.ToListAsync();
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        return await Students.FindAsync(id);
    }

    public async Task<Grade> GetGradeByIdAsync(int id)
    {
        return await Grades.FindAsync(id);
    }
}