﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using students1.Data;
using students1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace students1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly SchoolDbContext _context;
        private readonly IConfiguration _config;

        public TeacherController(SchoolDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

       // [Authorize(Roles = "Teacher, Admin")]
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
                Password = teacher.Password,
                Role = "Teacher"
            };
            _context.Teachers.Add(t);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        // [Authorize(Roles = "Teacher, Admin")]
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] LoginTeacher teacher,[FromQuery] IdentityRole role)
        {
            var teacherExists = _context.Teachers.FirstOrDefault(t => t.Email == teacher.Email && t.Password == teacher.Password);
            if (teacherExists == null)
            {
                return BadRequest("Invalid email or password.");
            }
            role = new IdentityRole { Name = "Teacher" };
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
   
