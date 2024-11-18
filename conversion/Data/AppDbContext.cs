namespace Conversion.Data

open Microsoft.EntityFrameworkCore
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc

type Student =
    {
        Id: int
        Name: string
        Age: int
        Grades: Grade list
    }

and Grade =
    {
        Id: int
        StudentId: int
        Subject: string
        Score: float
    }

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    member val Students: DbSet<Student> = base.Set<Student>() with get, set
    member val Grades: DbSet<Grade> = base.Set<Grade>() with get, set

    override this.OnModelCreating(modelBuilder: ModelBuilder) =
        modelBuilder.Entity<Student>().ToTable("Students")
        modelBuilder.Entity<Grade>().ToTable("Grades")

type StudentsController(context: AppDbContext) =
    inherit ControllerBase()

    [<HttpGet("students/{id}/grades")>]
    member this.GetStudentWithGrades(id: int) : Task<IActionResult> =
        if id <= 0 then
            return this.BadRequest("Invalid student ID.") :> IActionResult
        task {
            let! student = context.Students.Include(fun s -> s.Grades).FirstOrDefaultAsync(fun s -> s.Id = id)
            if student = null then
                return this.NotFound() :> IActionResult
            return this.Ok({ student with Grades = if student.Grades = null then [] else student.Grades }) :> IActionResult
        }