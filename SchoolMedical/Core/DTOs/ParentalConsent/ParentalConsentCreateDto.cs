using System;

namespace SchoolMedical.Core.DTOs.ParentalConsent
{
	public class ParentalConsentCreateDto
	{
		public int StudentID { get; set; }
		public int? VaccinationEventID { get; set; }
		public int? CheckupID { get; set; }
		public int ParentID { get; set; }
		public string ConsentStatus { get; set; }
		public DateTime? ConsentDate { get; set; }
		public string? Note { get; set; }
		public string ConsentType { get; set; } // "Vaccination" hoặc "Checkup"
	}
} 