package Interfaces;

using System;
using System.Threading.Tasks;
using YourNamespace.Dtos;
using YourNamespace.Data;
using Microsoft.EntityFrameworkCore;

public interface IStudentService
{
    Task CreateStudentAsync(StudentDto studentDto);
}

public class StudentService : IStudentService
{
    private readonly YourDbContext _context;

    public StudentService(YourDbContext context)
    {
        _context = context;
    }

    public async Task CreateStudentAsync(StudentDto studentDto)
    {
        if (string.IsNullOrWhiteSpace(studentDto.Name) || 
            string.IsNullOrWhiteSpace(studentDto.Email) || 
            studentDto.Age <= 0 || 
            string.IsNullOrWhiteSpace(studentDto.Address))
        {
            throw new ArgumentException("StudentDto fields cannot be null or empty.");
        }

        if (await _context.Students.AnyAsync(s => s.Email == studentDto.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var student = new Student
        {
            Name = studentDto.Name,
            Email = studentDto.Email,
            Age = studentDto.Age,
            Address = studentDto.Address
        };

        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
    }
}