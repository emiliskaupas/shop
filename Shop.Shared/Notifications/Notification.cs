namespace Shop.Shared.Notifications;

/// <summary>
/// Types of notifications
/// </summary>
public enum NotificationType
{
    Info,
    Success,
    Warning,
    Error
}

/// <summary>
/// Represents a notification message
/// </summary>
public class Notification
{
    public string Message { get; init; } = string.Empty;
    public NotificationType Type { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string? Title { get; init; }
    public Dictionary<string, object> Metadata { get; init; } = new();

    /// <summary>
    /// Creates an info notification
    /// </summary>
    public static Notification Info(string message, string? title = null) =>
        new() { Message = message, Type = NotificationType.Info, Title = title };

    /// <summary>
    /// Creates a success notification
    /// </summary>
    public static Notification Success(string message, string? title = null) =>
        new() { Message = message, Type = NotificationType.Success, Title = title };

    /// <summary>
    /// Creates a warning notification
    /// </summary>
    public static Notification Warning(string message, string? title = null) =>
        new() { Message = message, Type = NotificationType.Warning, Title = title };

    /// <summary>
    /// Creates an error notification
    /// </summary>
    public static Notification Error(string message, string? title = null) =>
        new() { Message = message, Type = NotificationType.Error, Title = title };
}

/// <summary>
/// Interface for notification services
/// </summary>
public interface INotificationService
{
    Task SendNotificationAsync(Notification notification);
    Task SendNotificationAsync(string message, NotificationType type = NotificationType.Info);
}

/// <summary>
/// Collection of notifications for a request/operation
/// </summary>
public class NotificationCollection
{
    private readonly List<Notification> _notifications = new();

    public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();
    public bool HasNotifications => _notifications.Count > 0;
    public bool HasErrors => _notifications.Any(n => n.Type == NotificationType.Error);
    public bool HasWarnings => _notifications.Any(n => n.Type == NotificationType.Warning);

    public void Add(Notification notification)
    {
        _notifications.Add(notification);
    }

    public void AddInfo(string message, string? title = null)
    {
        Add(Notification.Info(message, title));
    }

    public void AddSuccess(string message, string? title = null)
    {
        Add(Notification.Success(message, title));
    }

    public void AddWarning(string message, string? title = null)
    {
        Add(Notification.Warning(message, title));
    }

    public void AddError(string message, string? title = null)
    {
        Add(Notification.Error(message, title));
    }

    public void Clear()
    {
        _notifications.Clear();
    }
}