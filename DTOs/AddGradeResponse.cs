package DTOs;

using System;

public class AddGradeResponse
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    
    private string subject;
    public string Subject 
    { 
        get => subject; 
        set 
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Subject cannot be null or empty.", nameof(Subject));
            subject = value; 
        } 
    }
    
    private decimal score;
    public decimal Score 
    { 
        get => score; 
        set 
        {
            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(Score), "Score must be between 0 and 100.");
            score = value; 
        } 
    }
    
    public DateTime Date { get; set; }
}