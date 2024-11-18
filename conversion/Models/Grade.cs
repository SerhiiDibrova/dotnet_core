namespace Conversion.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public decimal Score { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}