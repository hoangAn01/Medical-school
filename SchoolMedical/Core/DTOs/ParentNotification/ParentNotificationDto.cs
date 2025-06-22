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
		public string? Title { get; set; }
		public string? Content { get; set; }
		public DateTime? SentDate { get; set; }
		public string? Status { get; set; }
		public string? NotificationType { get; set; }
		public int? CheckupID { get; set; }
	}
} 