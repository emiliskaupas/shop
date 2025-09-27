using Microsoft.AspNetCore.Mvc;
using backend.Services;
using Shop.Shared.Notifications;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly NotificationService notificationService;

    public NotificationController(NotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    [HttpGet("latest")]
    public IActionResult GetLatestNotification()
    {
        var notification = notificationService.GetLatestNotification();

        if (notification == null)
            return NotFound(new { message = "No notifications found" });

        return Ok(notification);
    }

    [HttpGet("all")]
    public IActionResult GetAllNotifications()
    {
        var notifications = notificationService.GetAllNotifications();
        return Ok(notifications);
    }

    [HttpGet("status")]
    public IActionResult GetNotificationStatus()
    {
        var latest = notificationService.GetLatestNotification();
        var count = notificationService.GetNotificationCount();

        return Ok(new
        {
            hasNotifications = latest != null,
            totalCount = count,
            latestNotification = latest,
            lastUpdated = latest?.CreatedAt
        });
    }

    [HttpGet("type/{type}")]
    public IActionResult GetNotificationsByType(NotificationType type)
    {
        var notifications = notificationService.GetNotificationsByType(type);
        return Ok(notifications);
    }

    [HttpDelete("clear")]
    public IActionResult ClearNotifications()
    {
        notificationService.ClearNotifications();
        return Ok(new { message = "All notifications cleared" });
    }
}