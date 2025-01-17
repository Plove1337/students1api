using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students1.Data;
using students1.Models;
using Npgsql;
using Microsoft.AspNetCore.Authorization;


namespace students1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public StudentController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Director")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            return await _context.Students.Include(s => s.Class).ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Teacher,Admin,Director")]
        public async Task<ActionResult<Student>> GetById(int id)
        {
            var student = await _context.Students.Include(s => s.Class).FirstOrDefaultAsync(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return student;
        }

        [HttpGet("class/{classId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<IEnumerable<Student>>> GetByClassId(int classId)
        {
            var teacherEmail = User.Identity.Name;
            var teacher = await _context.Teachers.Include(t => t.Classes).FirstOrDefaultAsync(t => t.Email == teacherEmail);
            if (teacher == null || !teacher.Classes.Any(c => c.Id == classId))
            {
                return Forbid();
            }

            return await _context.Students.Where(s => s.ClassID == classId).ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Director")]
        public async Task<ActionResult<Student>> Create(CreateStudent student)
        {
            if (student.Age <= 0)
            {
                return BadRequest("Age must be a positive number.");
            }
            var calss = _context.Classes.Find(student.ClassID);
            if (calss == null)
            {
                return BadRequest("Class not found.");
            }
            var s = new Student();
            s.Name = student.Name;
            s.Surname = student.Surname;
            s.Age = student.Age;
            s.ClassID = student.ClassID;

            _context.Students.Add(s);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = s.Id }, student);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Director")]
        public async Task<IActionResult> Update(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch.");
            }

            if (!_context.Students.Any(e => e.Id == id))
            {
                return NotFound("Student not found.");
            }

            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Director")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}