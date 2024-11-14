package conversion.Data;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    private readonly DbContext _context;

    public DbSet<Student> Students { get; set; }

    public AppDbContext(string connectionString) : base(new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options)
    {
        _context = this;
        Students = _context.Set<Student>();
    }

    public void AddStudent(Student student)
    {
        if (student == null) throw new ArgumentNullException(nameof(student));
        _context.Students.Add(student);
        _context.SaveChanges();
    }

    public Student GetStudent(int id)
    {
        return _context.Students.Find(id);
    }

    public void UpdateStudent(Student student)
    {
        if (student == null) throw new ArgumentNullException(nameof(student));
        _context.Students.Update(student);
        _context.SaveChanges();
    }

    public void DeleteStudent(int id)
    {
        var student = _context.Students.Find(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            _context.SaveChanges();
        }
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}