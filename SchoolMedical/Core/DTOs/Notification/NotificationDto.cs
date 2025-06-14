using SchoolMedical.Core.DTOs.ParentNotification;
using System;
using System.Collections.Generic;

namespace SchoolMedical.Core.DTOs.Notification
{
    public class NotificationDto
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime SentDate { get; set; }
        public string? Status { get; set; }
        public List<ParentNotificationDto> ParentNotifications { get; set; } = new List<ParentNotificationDto>();
    }
} 