using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using students1.Data;
using students1.Models;
using System.Text;

namespace students1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        SchoolDbContext _context = new SchoolDbContext();

        public TeacherController(SchoolDbContext context)
        {
            _context = context;
        }


        [HttpPost("[action]")]
        public IActionResult Register([FromBody]CreateTeacher teacher)
        {
            var teacherExists = _context.Teachers.FirstOrDefault(t => t.Email == teacher.Email);
            if (teacherExists != null)
            {
                return BadRequest("Teacher already exists.");
            }
            var t = new Teacher();
            t.Name = teacher.Name;
            t.Surname = teacher.Surname;
            t.Email = teacher.Email;
            t.Password = teacher.Password;
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
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zaq1@WSX"));
            return Ok(teacherExists);
        }
    }
}   
