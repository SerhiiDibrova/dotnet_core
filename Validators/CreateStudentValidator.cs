namespace YourNamespace.Validators
{
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateStudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }

    public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
    {
        private readonly YourDbContext _context;

        public CreateStudentValidator(YourDbContext context)
        {
            _context = context;

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email format is invalid.")
                .MustAsync(BeUniqueEmail).WithMessage("The provided email is already in use.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("DateOfBirth is required.")
                .Must(BeAtLeast18YearsOld).WithMessage("Student must be at least 18 years old.")
                .Must(BeNotInFuture).WithMessage("DateOfBirth cannot be in the future.");

            RuleFor(x => x.EnrollmentDate)
                .NotEmpty().WithMessage("EnrollmentDate is required.")
                .Must(BeNotInFuture).WithMessage("EnrollmentDate cannot be in the future.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return !await _context.Students.AnyAsync(s => s.Email == email, cancellationToken);
        }

        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }

        private bool BeNotInFuture(DateTime date)
        {
            return date <= DateTime.Today;
        }
    }
}