package Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

public class Grade
{
    [Key]
    public int GradeId { get; set; }

    [Required]
    public int StudentId { get; set; }

    [Required]
    [StringLength(100)]
    public string Subject { get; set; }

    [Range(0, 100)]
    public decimal Score { get; set; }

    [Required]
    public DateTime Date { get; set; }
}

public class ApplicationDbContext : DbContext
{
    public DbSet<Grade> Grades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString"));
    }

    public List<Grade> GetGrades()
    {
        return Grades.ToList();
    }

    public List<Grade> GetGradesByStudentId(int studentId)
    {
        return Grades.Where(g => g.StudentId == studentId).ToList();
    }

    public List<Grade> GetGradesBySubject(string subject)
    {
        return Grades.Where(g => g.Subject == subject).ToList();
    }

    public void AddGrade(Grade grade)
    {
        try
        {
            Grades.Add(grade);
            SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void UpdateGrade(Grade grade)
    {
        try
        {
            Grades.Update(grade);
            SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void DeleteGrade(int gradeId)
    {
        try
        {
            var grade = Grades.Find(gradeId);
            if (grade != null)
            {
                Grades.Remove(grade);
                SaveChanges();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}

[ApiController]
[Route("api/[controller]")]
public class GradesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GradesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<List<Grade>> GetGrades()
    {
        try
        {
            var grades = _context.GetGrades();
            return Ok(grades);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }
    }
}