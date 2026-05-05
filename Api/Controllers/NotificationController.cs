using Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController(IGetNotificationsServer notificationsServer) : ControllerBase
{
    [HttpGet("getNotifications")]
    public async Task<IActionResult> GetNotifications([FromQuery] Guid userId)
    {
        var notifications = await notificationsServer.GetAllNotificationsByUserIdAsync(userId);
        return Ok(notifications);
    }
}