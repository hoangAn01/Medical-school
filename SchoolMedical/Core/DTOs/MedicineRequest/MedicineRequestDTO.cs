using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicineRequest
{
	public class MedicineRequestDTO
	{
		public int RequestID { get; set; }

		public DateTime Date { get; set; }

		[Required]
		[StringLength(100)]
		public string MedicineName { get; set; } = string.Empty;

		[StringLength(50)]
		public string? RequestStatus { get; set; }

		public int StudentID { get; set; }

		public int? ParentID { get; set; }

		[StringLength(255)]
		public string? AllergenCheck { get; set; }

		public int? ApprovedBy { get; set; } = null;

		public DateTime? ApprovalDate { get; set; }

		// Navigation properties represented as strings
		public string? StudentName { get; set; }
		public string? ParentName { get; set; }
	}
}