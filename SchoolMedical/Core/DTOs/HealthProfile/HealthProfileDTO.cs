using System;

namespace SchoolMedical.Core.DTOs.HealthProfile
{
	public class HealthProfileDTO
	{
		public int ProfileID { get; set; }
		public int? StudentID { get; set; }
		public string? ChronicDisease { get; set; }
		public string? VisionTest { get; set; }
		public string? Allergy { get; set; }
		public decimal? Weight { get; set; }
		public decimal? Height { get; set; }
		public DateTime? LastCheckupDate { get; set; }

		// Optionally include student info
		public string? StudentFullName { get; set; }
	}
}