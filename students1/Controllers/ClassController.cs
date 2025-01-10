using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using students1.Data;
using students1.Models;
using Npgsql;


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
        public async Task<ActionResult<IEnumerable<Class>>> GetAll()
        {
            return await _context.Classes.Include(c => c.Students).ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetById(int id)
        {
            var classModel = await _context.Classes.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == id);
            if (classModel == null)
            {
                return NotFound("Class not found.");
            }
            return classModel;
        }

        
        [HttpPost]
        public async Task<ActionResult<Class>> Create([FromBody] Class classModel)
        {
            if (classModel == null)
            {
                return BadRequest("Class data is required.");
            }

            if (string.IsNullOrWhiteSpace(classModel.Description))
            {
                return BadRequest("Description is required.");
            }

            _context.Classes.Add(classModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = classModel.Id }, classModel);
        }

        
        [HttpPut("{id}")]
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
