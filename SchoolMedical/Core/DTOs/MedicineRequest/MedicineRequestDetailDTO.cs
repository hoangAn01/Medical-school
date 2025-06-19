using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicineRequest
{
	public class MedicineRequestDetailDTO
	{
		public int RequestDetailID { get; set; }
		
		public int RequestID { get; set; }
		
		public string ItemName { get; set; } = string.Empty;
		
		public int Quantity { get; set; }
		
		[StringLength(255)]
		public string? DosageInstructions { get; set; }
		
		public string? Time { get; set; }
	}
}