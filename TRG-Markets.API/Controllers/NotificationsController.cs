using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TRG_Markets.Application.Interfaces;

namespace TRG_Markets.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _notificationService.GetALLAsync();
        return Ok(items);
    }
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var notification = await _notificationService.GetByIdAsync(id);
        if (notification == null) return NotFound();
        return Ok(notification);
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnread()
    {
        var items = await _notificationService.GetUnreadAsync();
        return Ok(items);
    }

    [HttpPut("{id}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var success = await _notificationService.MarkAsReadAsync(id);
        if (!success) return NotFound();
        return NoContent();

    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TRG_Markets.Domain.Entities.Notification notification)
    {
        if (notification == null) return BadRequest();

        var created = await _notificationService.CreateAsync(notification);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _notificationService.DeleteAsync(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

}
