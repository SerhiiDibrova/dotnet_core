package conversion.Models;

using System;
using System.Text.Json.Serialization;

public class GradeDto
{
    public int GradeId { get; set; }
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }
}