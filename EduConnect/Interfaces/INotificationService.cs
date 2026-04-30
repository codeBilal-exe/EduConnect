using EduConnect.Models;

namespace EduConnect.Interfaces;

// SRP: Only manages notifications
// DIP: Components depend on this interface for notifications
public interface INotificationService
{
    event Action<Notification>? OnNewNotification;
    event Action<Guid>? OnNotificationMarkedAsRead;

    void SendNotification(Notification notification);
    List<Notification> GetNotificationsForUser(Guid userId);
    void MarkAsRead(Guid notificationId);
    int GetUnreadCount(Guid userId);
}
