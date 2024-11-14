package Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public interface IStudentRepository
{
    Task<Student> GetStudentByIdAsync(int id);
    Task<List<Grade>> GetGradesByStudentIdAsync(int id);
}

public class StudentRepository : IStudentRepository
{
    private readonly DbContext _context;
    private readonly ILogger<StudentRepository> _logger;

    public StudentRepository(DbContext context, ILogger<StudentRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        try
        {
            return await _context.Set<Student>().FindAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student with id {Id}", id);
            throw;
        }
    }

    public async Task<List<Grade>> GetGradesByStudentIdAsync(int id)
    {
        try
        {
            var student = await GetStudentByIdAsync(id);
            if (student != null)
            {
                return await _context.Set<Grade>().Where(g => g.StudentId == id).ToListAsync();
            }
            return new List<Grade>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving grades for student with id {Id}", id);
            throw;
        }
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Grade
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public string Score { get; set; }
}