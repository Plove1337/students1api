using Docker.DotNet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using students1.Models;
using System.Security.Claims;

namespace students1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        
        private readonly IJwtAuthManager _jwtAuthManager;

        

        public AuthController(IJwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost("login-student")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Role, "Student")
            };

            var tokens = _jwtAuthManager.GenerateTokens(request.Email, claims, DateTime.UtcNow, "Student");

            return Ok(tokens);
        }

        [HttpPost("login-admin")]
        public IActionResult LoginAdmin([FromBody] LoginAdmin request)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var tokens = _jwtAuthManager.GenerateTokens(request.Email, claims, DateTime.UtcNow, "Admin");

            return Ok(tokens);
        }
        [HttpPost("login-headteacher")]
        public IActionResult LoginHeadteacher([FromBody] LoginAdmin request)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Role, "Director")
            };

            var tokens = _jwtAuthManager.GenerateTokens(request.Email, claims, DateTime.UtcNow, "Director");

            return Ok(tokens);
        }
        [HttpPost("login-teacher")]
        public IActionResult LoginTeacher([FromBody] LoginAdmin request)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Role, "Teacher")
            };

            var tokens = _jwtAuthManager.GenerateTokens(request.Email, claims, DateTime.UtcNow, "Admin");

            return Ok(tokens);
       
        
        }
    }
}
