package DTOs;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ApiResponseDto
{
    [JsonPropertyName("Student")]
    public StudentDto Student { get; set; }
    
    [JsonPropertyName("Grades")]
    public List<GradeDto> Grades { get; set; } = new List<GradeDto>();
}

public class StudentDto
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    
    [JsonPropertyName("Email")]
    [EmailAddress]
    public string Email { get; set; }
    
    [JsonPropertyName("DateOfBirth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
}

public class GradeDto
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    
    [JsonPropertyName("Subject")]
    public string Subject { get; set; }
    
    [JsonPropertyName("Score")]
    public decimal Score { get; set; }
    
    [JsonPropertyName("Term")]
    public string Term { get; set; }
}