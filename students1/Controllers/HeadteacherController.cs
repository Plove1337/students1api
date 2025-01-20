using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using students1.Data;
using students1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace students1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeadteacherController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public HeadteacherController(SchoolDbContext context)
        {
            _context = context;
        }


        [HttpPost("[action]")]
        public IActionResult Register([FromBody] CreateHeadteacher headteacher)
        {
            var headteacherExists = _context.Headteachers.FirstOrDefault(h => h.Email == headteacher.Email);
            if (headteacherExists != null)
            {
                return BadRequest("Headteacher already exists.");
            }
            var h = new Headteacher
            {
                Name = headteacher.Name,
                Surname = headteacher.Surname,
                Email = headteacher.Email,
                Password = headteacher.Password,
                Role = "Headteacher"
            };
            _context.Headteachers.Add(h);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginHeadteacher headteacher)
        {
            var headteacherExists = _context.Headteachers.FirstOrDefault(h => h.Email == headteacher.Email && h.Password == headteacher.Password);
            if (headteacherExists == null)
            {
                return BadRequest("Invalid email or password.");
            }
            var token = GenerateJwtToken();
            return Ok(new { token });
        }
        private string GenerateJwtToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JwtOptions:SigningKey"));
            var expires = DateTime.Now.AddMinutes(30);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5039",
                audience: "https://localhost:5039",
                claims: new List<Claim>(),
                expires: expires,
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
