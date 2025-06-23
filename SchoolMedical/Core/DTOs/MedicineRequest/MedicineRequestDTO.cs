using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicineRequest
{
	public class MedicineRequestDTO
	{
		public int RequestID { get; set; }
		
		public DateTime Date { get; set; }
		
		public string? RequestStatus { get; set; }
		
		public int StudentID { get; set; }
		
		public int? ParentID { get; set; }
		
		[StringLength(255)]
		public string? Note { get; set; }
		
		[StringLength(50)]
		public string? NurseNote { get; set; }
		
		public int? ApprovedBy { get; set; }
		
		public DateTime? ApprovalDate { get; set; }

		// Navigation properties represented as strings
		public string? StudentName { get; set; }
		public string? ParentName { get; set; }

		// Collection of medicine details
		public List<MedicineRequestDetailDTO> MedicineDetails { get; set; } = new();
	}
}