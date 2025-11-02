using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Tags("Users")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet]
    [Authorize] // требуется JWT
    [ProducesResponseType(typeof(object[]), StatusCodes.Status200OK)]
    public IActionResult GetAll() => Ok(new[] { new { id = 1, name = "Alice" } });
}

