using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicalInventory
{
	public class CreateMedicalInventoryRequest
	{
		[Required]
		[StringLength(100)]
		public string ItemName { get; set; } = string.Empty;

		[StringLength(50)]
		public string? Category { get; set; }

		public int? Quantity { get; set; }

		[StringLength(20)]
		public string? Unit { get; set; }

		[StringLength(255)]
		public string? Description { get; set; }
	}
}