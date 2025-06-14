using System;

namespace SchoolMedical.Core.DTOs.ParentNotification
{
    public class ParentNotificationDto
    {
        public int NotificationID { get; set; }
        public int ParentID { get; set; }
        public string ParentName { get; set; } // For displaying parent name
        public string NotificationTitle { get; set; } // For displaying notification title
        public DateTime IndividualSentDate { get; set; }
        public string? IndividualStatus { get; set; }
    }
} 