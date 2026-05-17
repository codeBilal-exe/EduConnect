using EduConnect.Models;

namespace EduConnect.Interfaces;

// SRP: Only manages notifications
// DIP: Components depend on this interface for notifications
public interface INotificationService
{
    event Action<Notification>? OnNewNotification;
    event Action<int>? OnNotificationMarkedAsRead;

    void SendNotification(Notification notification);
    List<Notification> GetNotificationsForUser(int studentId);
    void MarkAsRead(int notificationId);
    int GetUnreadCount(int studentId);
}
