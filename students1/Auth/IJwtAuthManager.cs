using Microsoft.IdentityModel.Tokens;
using students1.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public interface IJwtAuthManager
{
    Tokens GenerateTokens(string username, Claim[] claims, DateTime now, string role);
    ClaimsPrincipal ValidateJwtToken(string token);
}

public class JwtAuthManager : IJwtAuthManager
{
    private readonly string _secret;
    private readonly string _adminSecret;
    private string v;

    public JwtAuthManager(string v)
    {
        this.v = v;
    }

    public JwtAuthManager(string secret, string adminSecret)
    {
        _secret = secret;
        _adminSecret = adminSecret;
    }

    public Tokens GenerateTokens(string username, Claim[] claims, DateTime now, string role)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(role == "Admin" ? _adminSecret : _secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "https://localhost:5039",
            audience: "https://localhost:5039",
            claims: claims,
            expires: now.AddMinutes(30),
            signingCredentials: creds);

        var refreshToken = Guid.NewGuid().ToString();

        return new Tokens
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken
        };
    }

    public ClaimsPrincipal ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secret);
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
