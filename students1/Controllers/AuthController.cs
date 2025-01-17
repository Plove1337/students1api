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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var role = "Student";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var tokens = _jwtAuthManager.GenerateTokens(request.Email, claims, DateTime.UtcNow, role);

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
    }
}
