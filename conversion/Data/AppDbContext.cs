namespace Conversion.Data

open Microsoft.EntityFrameworkCore
open System.Threading.Tasks
open System

type Student =
    {
        Id: int
        Name: string
        Age: int
    }

type Grade =
    {
        Id: int
        StudentId: int
        Subject: string
        Score: int
    }

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    member val Students : DbSet<Student> = base.Set<Student>() with get, set
    member val Grades : DbSet<Grade> = base.Set<Grade>() with get, set

    override this.OnModelCreating(modelBuilder: ModelBuilder) =
        modelBuilder.Entity<Student>().HasKey(fun s -> s.Id) |> ignore
        modelBuilder.Entity<Grade>().HasKey(fun g -> g.Id) |> ignore
        modelBuilder.Entity<Grade>()
            .HasOne<Student>()
            .WithMany()
            .HasForeignKey(fun g -> g.StudentId) |> ignore
        modelBuilder.Entity<Student>().Property(fun s -> s.Name).IsRequired() |> ignore
        modelBuilder.Entity<Student>().Property(fun s -> s.Age).IsRequired() |> ignore
        modelBuilder.Entity<Grade>().Property(fun g -> g.Subject).IsRequired() |> ignore
        modelBuilder.Entity<Grade>().Property(fun g -> g.Score).IsRequired() |> ignore

    member this.GetStudentById(id: int) : Task<Option<Student>> =
        if id <= 0 then
            Task.FromResult(None)
        else
            this.Students.FindAsync(id) |> Async.AwaitTask |> Task.FromResult << Option.ofObj