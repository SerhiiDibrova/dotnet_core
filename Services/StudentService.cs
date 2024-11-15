namespace YourNamespace.Services

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using Microsoft.Extensions.Logging;

public class StudentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(AppDbContext context, ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ActionResult<string>> CreateStudentAsync(Student student)
    {
        if (student == null)
        {
            return new BadRequestObjectResult("Student data is required.");
        }

        var validationResults = ValidateStudent(student);
        if (validationResults.Any())
        {
            return new BadRequestObjectResult(validationResults);
        }

        if (await _context.Students.AnyAsync(s => s.Email == student.Email))
        {
            return new ConflictObjectResult("Email must be unique.");
        }

        try
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Student created successfully: {StudentId}, Name: {StudentName}, Email: {StudentEmail}", student.Id, student.Name, student.Email);
            return new OkObjectResult("Student created successfully.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error occurred while creating student: {StudentId}, Name: {StudentName}, Email: {StudentEmail}", student.Id, student.Name, student.Email);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating student: {StudentId}, Name: {StudentName}, Email: {StudentEmail}", student.Id, student.Name, student.Email);
            return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
        }
    }

    private IEnumerable<string> ValidateStudent(Student student)
    {
        var results = new List<string>();
        if (string.IsNullOrWhiteSpace(student.Name))
        {
            results.Add("Name is required.");
        }
        if (string.IsNullOrWhiteSpace(student.Email) || !IsValidEmail(student.Email))
        {
            results.Add("Valid email is required.");
        }
        if (student.DateOfBirth == default)
        {
            results.Add("Date of birth is required.");
        }
        if (student.DateOfBirth > DateTime.Now)
        {
            results.Add("Date of birth must be in the past.");
        }
        if (string.IsNullOrWhiteSpace(student.Address))
        {
            results.Add("Address is required.");
        }
        if (string.IsNullOrWhiteSpace(student.PhoneNumber))
        {
            results.Add("Phone number is required.");
        }
        return results;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}