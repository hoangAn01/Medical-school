using System;

namespace SchoolMedical.Core.DTOs.ParentNotification
{
	public class ParentNotificationDetailDto
	{
		public int NotificationID { get; set; }
		public int ParentID { get; set; }
		public string ParentName { get; set; }
		public string NotificationTitle { get; set; }
		public string NotificationContent { get; set; }
		public DateTime IndividualSentDate { get; set; }
		public string? IndividualStatus { get; set; }
		public DateTime NotificationSentDate { get; set; }
		public string? NotificationStatus { get; set; }
	}
} 