using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entity
{
	[Table("HealthProfile")]
	public class HealthProfile
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)] // Vì bạn đã set NOT NULL mà không phải IDENTITY
		public int ProfileID { get; set; }

		public int? StudentID { get; set; }

		[MaxLength(255)]
		public string? ChronicDisease { get; set; }

		[MaxLength(255)]
		public string? VisionTest { get; set; }

		[MaxLength(255)]
		public string? Allergy { get; set; }
	}
}
