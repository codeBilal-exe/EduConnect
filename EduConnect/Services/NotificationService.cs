using EduConnect.Data;
using EduConnect.Interfaces;
using EduConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace EduConnect.Services;

public class NotificationService : INotificationService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public event Action<Notification>? OnNewNotification;
    public event Action<int>? OnNotificationMarkedAsRead;

    public NotificationService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public void SendNotification(Notification notification)
    {
        using var context = _dbContextFactory.CreateDbContext();
        context.Notifications.Add(notification);
        context.SaveChanges();
        OnNewNotification?.Invoke(notification);
    }

    public List<Notification> GetNotificationsForUser(int studentId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.Notifications
            .Where(n => n.StudentId == studentId)
            .OrderByDescending(n => n.CreatedAt)
            .AsNoTracking()
            .ToList();
    }

    public void MarkAsRead(int notificationId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        var notification = context.Notifications.FirstOrDefault(n => n.Id == notificationId);
        if (notification == null)
            return;

        notification.IsRead = true;
        context.SaveChanges();
        OnNotificationMarkedAsRead?.Invoke(notificationId);
    }

    public int GetUnreadCount(int studentId)
    {
        using var context = _dbContextFactory.CreateDbContext();
        return context.Notifications.Count(n => n.StudentId == studentId && !n.IsRead);
    }
}
