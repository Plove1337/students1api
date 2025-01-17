using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students1.Data;
using students1.Models;
using Npgsql;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace students1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public ClassController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<IEnumerable<Class>>> GetAll()
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userRole == "Admin")
            {
                return await _context.Classes.Include(c => c.Students).ToListAsync();
            }
            else if (userRole == "Teacher")
            {
                var teacher = await _context.Teachers.Include(t => t.Classes).FirstOrDefaultAsync(t => t.Id == userId);
                if (teacher == null)
                {
                    return NotFound("Teacher not found.");
                }
                return Ok(teacher.Classes);
            }
            return Forbid();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<ActionResult<Class>> GetById(int id)
        {
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var classModel = await _context.Classes.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == id);
            if (classModel == null)
            {
                return NotFound("Class not found.");
            }

            if (userRole == "Admin")
            {
                return classModel;
            }
            else if (userRole == "Teacher")
            {
                var teacher = await _context.Teachers.Include(t => t.Classes).FirstOrDefaultAsync(t => t.Id == userId);
                if (teacher == null || !teacher.Classes.Any(c => c.Id == id))
                {
                    return Forbid();
                }
                return classModel;
            }
            return Forbid();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Director")]
        public async Task<ActionResult<Class>> Create(CreateClass classModel)
        {
            if (classModel == null)
            {
                return BadRequest("Class data is required.");
            }

            if (string.IsNullOrWhiteSpace(classModel.Description))
            {
                return BadRequest("Description is required.");
            }

            var c = new Class
            {
                Description = classModel.Description,
                Name = classModel.Name,
                Students = classModel.Students
            };

            _context.Classes.Add(c);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = c.Id }, classModel);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Class classModel)
        {
            if (id != classModel.Id)
            {
                return BadRequest("Class ID mismatch.");
            }

            if (string.IsNullOrWhiteSpace(classModel.Description))
            {
                return BadRequest("Description is required.");
            }

            if (!_context.Classes.Any(c => c.Id == id))
            {
                return NotFound("Class not found.");
            }

            _context.Entry(classModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var classModel = await _context.Classes.FindAsync(id);
            if (classModel == null)
            {
                return NotFound("Class not found.");
            }

            _context.Classes.Remove(classModel);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
