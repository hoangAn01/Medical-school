using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicineRequest
{
	public class MedicineRequestCreateRequest
	{
		[Required]
		[StringLength(100)]
		public string MedicineName { get; set; } = string.Empty;

		[Required]
		public int StudentID { get; set; }

		public int? ParentID { get; set; }

		[StringLength(255)]
		public string? AllergenCheck { get; set; }
	}
}