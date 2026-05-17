using EduConnect.Models.Enums;

namespace EduConnect.Models;

public class Notification
{
    public int Id { get; set; } = 0;
    public required int StudentId { get; set; }
    public required string Message { get; set; }
    public required NotificationType Type { get; set; }
    public required bool IsRead { get; set; }
    public required DateTime CreatedAt { get; set; }
}
