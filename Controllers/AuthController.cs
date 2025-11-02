using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[Tags("Auth")]
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _cfg;
    public AuthController(IConfiguration cfg) => _cfg = cfg;

    [HttpPost("login")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Login([FromBody] object _)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[] { new Claim(ClaimTypes.Name, "demo-user") },
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { access_token = tokenString });
    }
}
