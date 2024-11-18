namespace YourProject.Conversion.DTOs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using YourProject.Data;
    using Microsoft.EntityFrameworkCore;

    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class StudentService
    {
        private readonly YourDbContext _context;
        private readonly IMapper _mapper;

        public StudentService(YourDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            try
            {
                var studentList = await _context.Students.ToListAsync();
                return _mapper.Map<List<StudentDto>>(studentList);
            }
            catch (DbUpdateException ex)
            {
                // Handle database update exception
                throw new Exception("An error occurred while retrieving students.", ex);
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }
}