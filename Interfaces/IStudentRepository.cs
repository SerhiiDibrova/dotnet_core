package Interfaces;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IStudentRepository
{
    Task<Student> GetStudentByIdAsync(int id);
    Task<List<Grade>> GetGradesByStudentIdAsync(int studentId);
}

public class StudentRepository : IStudentRepository
{
    private readonly DatabaseContext _context;

    public StudentRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        return await _context.Students.FindAsync(id);
    }

    public async Task<List<Grade>> GetGradesByStudentIdAsync(int studentId)
    {
        return await _context.Grades.Where(g => g.StudentId == studentId).ToListAsync();
    }
}