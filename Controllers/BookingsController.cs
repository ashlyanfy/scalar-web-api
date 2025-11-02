using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Tags("Bookings")]                      
[ApiController]
[Route("api/[controller]")]             
public class BookingsController : ControllerBase
{
  
    [HttpGet]                          
    [Authorize]
    public IActionResult GetAll() =>
        Ok(new[] { new { id = 101, userId = 1, resourceId = 1 } });


    [HttpGet("{id:int}")]           
    public IActionResult GetById(int id) =>
        Ok(new { id, userId = 1, resourceId = 1 });

    [HttpPost]                       
    [Authorize]
    public IActionResult Create([FromBody] BookingCreateDto dto) =>
        CreatedAtAction(nameof(GetById), new { id = 999 }, dto);
}

public record BookingCreateDto(int ResourceId, int UserId, DateTime StartAt);
