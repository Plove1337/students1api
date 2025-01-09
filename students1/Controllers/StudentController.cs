using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students1.Data;
using students1.Models;

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

        public async Task<ActionResult<IEnumerable<Student>>> GetAll()
        {
            return await _context.Students.Include(u => u.Class).ToListAsync();
        }

        [HttpGet("{ClassId}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetByClass(int ClassId)
        {
            return await _context.Students.Where(u => u.ClassID == ClassId).ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<Student>> Add(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = student.Id }, student);
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var student = await _context.Students.FindAsync(Id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
