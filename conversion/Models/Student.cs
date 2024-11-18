namespace Conversion.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Range(0, 120)]
        public int Age { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public Student() { }
    }
}

namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Conversion.Models;

    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }
    }
}