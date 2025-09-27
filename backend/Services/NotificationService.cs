using Shop.Shared.Notifications;

namespace backend.Services;

/// <summary>
/// In-memory notification service for storing and retrieving notifications
/// </summary>
public class NotificationService : INotificationService
{
    private readonly List<Notification> _notifications = new();
    private readonly object _lock = new();

    public async Task SendNotificationAsync(Notification notification)
    {
        lock (_lock)
        {
            _notifications.Add(notification);
        }
        await Task.CompletedTask;
    }

    public async Task SendNotificationAsync(string message, NotificationType type = NotificationType.Info)
    {
        var notification = type switch
        {
            NotificationType.Info => Notification.Info(message),
            NotificationType.Success => Notification.Success(message),
            NotificationType.Warning => Notification.Warning(message),
            NotificationType.Error => Notification.Error(message),
            _ => Notification.Info(message)
        };

        await SendNotificationAsync(notification);
    }

    /// <summary>
    /// Get the latest notification
    /// </summary>
    public Notification? GetLatestNotification()
    {
        lock (_lock)
        {
            return _notifications.OrderByDescending(n => n.CreatedAt).FirstOrDefault();
        }
    }

    /// <summary>
    /// Get all notifications ordered by creation date (newest first)
    /// </summary>
    public IReadOnlyList<Notification> GetAllNotifications()
    {
        lock (_lock)
        {
            return _notifications.OrderByDescending(n => n.CreatedAt).ToList().AsReadOnly();
        }
    }

    /// <summary>
    /// Get notifications by type
    /// </summary>
    public IReadOnlyList<Notification> GetNotificationsByType(NotificationType type)
    {
        lock (_lock)
        {
            return _notifications
                .Where(n => n.Type == type)
                .OrderByDescending(n => n.CreatedAt)
                .ToList()
                .AsReadOnly();
        }
    }

    /// <summary>
    /// Clear all notifications
    /// </summary>
    public void ClearNotifications()
    {
        lock (_lock)
        {
            _notifications.Clear();
        }
    }

    /// <summary>
    /// Get notification count
    /// </summary>
    public int GetNotificationCount()
    {
        lock (_lock)
        {
            return _notifications.Count;
        }
    }
}