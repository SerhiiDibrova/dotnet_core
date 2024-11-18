namespace Conversion.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Conversion.Dtos;
    using Conversion.Services;
    using Microsoft.Extensions.Logging;
    using FluentValidation;

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;
        private readonly IValidator<CreateStudentDto> _validator;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger, IValidator<CreateStudentDto> validator)
        {
            _studentService = studentService;
            _logger = logger;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {
            var validationResult = await _validator.ValidateAsync(createStudentDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var student = await _studentService.CreateStudentAsync(createStudentDto);
                return CreatedAtAction(nameof(CreateStudent), new { id = student.Id }, student);
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, "Validation error occurred while creating student.");
                return BadRequest(vex.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating student.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}