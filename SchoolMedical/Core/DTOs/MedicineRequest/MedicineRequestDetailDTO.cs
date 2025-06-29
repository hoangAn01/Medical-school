using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicineRequest
{
	public class MedicineRequestDetailDTO
	{
		public int RequestDetailID { get; set; }
		
		// public int RequestID { get; set; }
		
		public int RequestItemID { get; set; }
		
		public string RequestItemName { get; set; } = string.Empty;
		
		public string? Description { get; set; }
		
		// public string? MedicineType { get; set; } // Add this line
		
		public int Quantity { get; set; }
		
		[StringLength(255)]
		public string? DosageInstructions { get; set; }
		
		public string? Time { get; set; }
	}
}