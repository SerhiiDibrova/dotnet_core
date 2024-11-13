using System;

namespace Models
{
    public class Student
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public string Address { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public DateTime EnrollmentDate { get; set; }
        
        public string Major { get; set; }
    }
}