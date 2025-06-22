namespace SchoolMedical.Core.DTOs.MedicalInventory
{
	public class MedicalInventoryDTO
	{
		public string ItemName { get; set; } = string.Empty;
		public string? Category { get; set; }
		public int? Quantity { get; set; }
		public string? Unit { get; set; }
		public string? Description { get; set; }
	}
}