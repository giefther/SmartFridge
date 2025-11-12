using System;

namespace SmartFridge.Core.Models
{
    public class Notification
    {
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public enum NotificationType
    {
        Info,
        Warning,
        Danger
    }
}