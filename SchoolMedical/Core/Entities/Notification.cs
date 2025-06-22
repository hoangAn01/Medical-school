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

		[StringLength(50)]
		public string? NotificationType { get; set; }

		// Liên kết đến sự kiện tiêm chủng (nếu có)
		public int? VaccinationEventID { get; set; }
		[ForeignKey("VaccinationEventID")]
		public VaccinationEvent? VaccinationEvent { get; set; }

		// Liên kết đến sự kiện y tế (nếu có)
		public int? MedicalEventID { get; set; }
		[ForeignKey("MedicalEventID")]
		public MedicalEvent? MedicalEvent { get; set; }

		// Liên kết đến kết quả khám (nếu có)
		public int? CheckupID { get; set; }
		[ForeignKey("CheckupID")]
		public SchoolCheckup? SchoolCheckup { get; set; }

		// Navigation property for the join table
		public ICollection<ParentNotification> ParentNotifications { get; set; } = new List<ParentNotification>();
	}
}