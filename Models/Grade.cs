package Models;

public class Grade {
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string Subject { get; set; }
    public decimal Score { get; set; }
    public string Term { get; set; }
}