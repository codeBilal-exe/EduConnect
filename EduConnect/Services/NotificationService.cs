using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;

namespace EduConnect.Services;

// SRP: Only manages notifications
// DIP: Implements INotificationService abstraction
public class NotificationService : INotificationService
{
    private readonly List<Notification> _notifications;

    // Event-driven: Components subscribe to this and re-render reactively
    public event Action<Notification>? OnNewNotification;

    public NotificationService()
    {
        _notifications = new List<Notification>(SeedData.Notifications);
    }

    // SRP: Sending notifications is a core responsibility
    public void SendNotification(Notification notification)
    {
        _notifications.Add(notification);
        // Fire event so all subscribers re-render (essential for Blazor reactivity)
        OnNewNotification?.Invoke(notification);
    }

    public List<Notification> GetNotificationsForUser(Guid userId)
    {
        return _notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
    }

    public void MarkAsRead(Guid notificationId)
    {
        var notification = _notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
        }
    }

    public int GetUnreadCount(Guid userId)
    {
        return _notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .Count();
    }
}
