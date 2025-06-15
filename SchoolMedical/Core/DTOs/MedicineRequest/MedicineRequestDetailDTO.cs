using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicineRequest
{
	public class MedicineRequestDetailDTO
	{
		public int RequestDetailID { get; set; }
		
		public int RequestID { get; set; }
		
		public int ItemID { get; set; }
		
		public int Quantity { get; set; }
		
		[StringLength(255)]
		public string? DosageInstructions { get; set; }
	}
}