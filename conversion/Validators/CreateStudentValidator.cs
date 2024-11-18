namespace Conversion.Validators
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;

    public class CreateStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

    public interface IStudentRepository
    {
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
    }

    public class CreateStudentValidator : AbstractValidator<CreateStudentDto>
    {
        private readonly IStudentRepository _studentRepository;

        public CreateStudentValidator(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;

            RuleFor(student => student.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(student => student.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");

            RuleFor(student => student.Age)
                .NotEmpty().WithMessage("Age is required.")
                .GreaterThan(0).WithMessage("Age must be greater than 0.")
                .LessThan(120).WithMessage("Age must be less than 120.");
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _studentRepository.IsEmailUniqueAsync(email, cancellationToken);
        }
    }
}