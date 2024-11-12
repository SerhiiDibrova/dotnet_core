package conversion.Models;

using System;
using Microsoft.AspNetCore.Mvc;

public class Grade
{
    public int GradeId { get; set; }
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }

    public static IActionResult ValidateStudentId(int? studentId)
    {
        if (!studentId.HasValue)
        {
            return new BadRequestObjectResult("studentId is required");
        }

        if (studentId <= 0)
        {
            return new BadRequestObjectResult("studentId must be a valid positive integer");
        }

        return null;
    }

    public static IActionResult ValidateSubject(string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
        {
            return new BadRequestObjectResult("Subject is required");
        }

        return null;
    }

    public static IActionResult ValidateScore(decimal score)
    {
        if (score < 0 || score > 100)
        {
            return new BadRequestObjectResult("Score must be between 0 and 100");
        }

        return null;
    }

    public static IActionResult ValidateDate(DateTime date)
    {
        if (date > DateTime.Now)
        {
            return new BadRequestObjectResult("Date cannot be in the future");
        }

        return null;
    }
}