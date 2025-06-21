using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicineRequest
{
	public class RequestItemListDTO
	{
		public int RequestItemID { get; set; }
		
		[Required]
		public string RequestItemName { get; set; } = string.Empty;
		
		public string? Description { get; set; }
	}
} 