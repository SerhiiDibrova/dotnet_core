namespace Conversion.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage;
    using System;

    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().ToTable("Students");
        }

        public void AddStudent(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Name) || student.Age <= 0 || string.IsNullOrWhiteSpace(student.Email))
            {
                throw new ArgumentException("Invalid student data.");
            }

            try
            {
                Students.Add(student);
                SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw new Exception("An error occurred while saving the student.");
            }
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}