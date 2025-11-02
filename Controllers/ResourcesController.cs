using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Tags("Resources")]                    
[ApiController]
[Route("api/[controller]")]             
public class ResourcesController : ControllerBase
{
    
    [HttpGet]                        
    public IActionResult GetAll() =>
        Ok(new[] { new { id = 1, title = "Handbook.pdf" } });

 
    [HttpPost]                          
    [Authorize]
    public IActionResult Create([FromBody] ResourceCreateDto dto) =>
        CreatedAtAction(nameof(GetAll), new { }, dto);
}


public record ResourceCreateDto(string Title, string Url);

