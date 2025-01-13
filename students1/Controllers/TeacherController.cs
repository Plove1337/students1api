using Microsoft.AspNetCore.Mvc;
using students1.Data;
using students1.Models;

namespace students1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        SchoolDbContext _context = new SchoolDbContext();
        private IConfiguration _config;
        public TeacherController(IConfiguration config)
        {
            _config = config;
        }


        [HttpPost("[action]")]
        public IActionResult Register([FromBody] CreateTeacher teacher)
        {
            var teacherExists = _context.Teachers.FirstOrDefault(t => t.Email == teacher.Email);
            if (teacherExists != null)
            {
                return BadRequest("Teacher already exists.");
            }
            var t = new Teacher
            {
                Name = teacher.Name,
                Surname = teacher.Surname,
                Email = teacher.Email,
                Password = teacher.Password
            };
            _context.Teachers.Add(t);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] Teacher teacher)
        {
            var teacherExists = _context.Teachers.FirstOrDefault(t => t.Email == teacher.Email && t.Password == teacher.Password);
            if (teacherExists == null)
            {
                return BadRequest("Invalid email or password.");
            }
            return Ok(teacherExists);
        }
    }
}   
