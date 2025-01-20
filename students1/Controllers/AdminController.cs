using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class AdminController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public AdminController(SchoolDbContext context)
        {
            _context = context;
        }
        [HttpPost("[action]")]
        public IActionResult Register([FromBody] CreateAdmin admin)
        {
            var adminExists = _context.Admins.FirstOrDefault(h => h.Email == admin.Email);
            if (adminExists != null)
            {
                return BadRequest("Headteacher already exists.");
            }
            var a = new Admin
            {
                Name = admin.Name,
                Surname = admin.Surname,
                Email = admin.Email,
                Password = admin.Password,
                Role = "Admin"
            };
            _context.Admins.Add(a);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] LoginAdmin admin)
        {
            var adminExists = _context.Admins.FirstOrDefault(a => a.Email == admin.Email && a.Password == admin.Password);
            if (adminExists == null)
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
