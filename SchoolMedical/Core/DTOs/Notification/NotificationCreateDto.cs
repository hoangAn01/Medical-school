using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.Notification
{
	public class NotificationCreateDto
	{
		[Required]
		[StringLength(100)]
		public string Title { get; set; }

		[Required]
		public string Content { get; set; }

		public string? Status { get; set; }

		[StringLength(50)]
		public string? NotificationType { get; set; }
	}
}