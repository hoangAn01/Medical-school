using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
	[Table("MedicineRequestDetail")]
	public class MedicineRequestDetail
	{
		[Key]
		public int RequestDetailID { get; set; }
		
		[Required]
		public int RequestID { get; set; }
		
		[Required]
		public int RequestItemID { get; set; }
		
		// public string? MedicineType { get; set; } // Add this line
		
		[Required]
		public int Quantity { get; set; }
		
		[StringLength(255)]
		public string? DosageInstructions { get; set; }
		
		public string? Time { get; set; }

		// Navigation properties
		[ForeignKey("RequestID")]
		public virtual MedicineRequest? MedicineRequest { get; set; }
		
		[ForeignKey("RequestItemID")]
		public virtual RequestItemList? RequestItem { get; set; }
	}
}