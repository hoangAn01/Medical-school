using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
	[Table("Notification")]
	public class Notification
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int NotificationID { get; set; }

		[Required]
		[StringLength(100)]
		public string Title { get; set; } = string.Empty;

		[Required]
		public string Content { get; set; } = string.Empty;

		public DateTime SentDate { get; set; }

		[StringLength(50)]
		public string? Status { get; set; }

		// Navigation property for the many-to-many relationship
		public ICollection<ParentNotification> ParentNotifications { get; set; } = new List<ParentNotification>();
	}
} 