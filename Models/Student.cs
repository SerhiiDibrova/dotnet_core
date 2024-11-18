namespace YourNamespace.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; }

        [Range(1, 120, ErrorMessage = "Age must be a positive integer.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public Student(int id, string name, int age, string email)
        {
            if (age <= 0) throw new ArgumentOutOfRangeException(nameof(age), "Age must be a positive integer.");
            Id = id;
            Name = name;
            Age = age;
            Email = email;
        }

        public void UpdateStudent(string name, int age, string email)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (age <= 0) throw new ArgumentOutOfRangeException(nameof(age), "Age must be a positive integer.");
            Name = name;
            Age = age;
            Email = email;
        }
    }
}