namespace Conversion.Validators
{
    using FluentValidation;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateStudentRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public interface IStudentRepository
    {
        Task<bool> IsEmailUniqueAsync(string email);
    }

    public class CreateStudentValidator : AbstractValidator<CreateStudentRequest>
    {
        private readonly IStudentRepository _studentRepository;

        public CreateStudentValidator(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");

            RuleFor(x => x.DateOfBirth)
                .NotNull().WithMessage("Date of Birth is required.")
                .Must(BeAtLeast18YearsOld).WithMessage("Student must be at least 18 years old.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _studentRepository.IsEmailUniqueAsync(email);
        }

        private bool BeAtLeast18YearsOld(DateTime? dateOfBirth)
        {
            if (!dateOfBirth.HasValue) return false;
            var age = DateTime.Today.Year - dateOfBirth.Value.Year;
            if (dateOfBirth.Value > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}