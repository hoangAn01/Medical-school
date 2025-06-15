using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
	[Table("VaccineRecord")]
	public class VaccineRecord
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int VaccineRecordID { get; set; }

		public int StudentID { get; set; }
		[ForeignKey("StudentID")]
		public Student Student { get; set; }

		public int? VaccinationEventID { get; set; }
		[ForeignKey("VaccinationEventID")]
		public VaccinationEvent? VaccinationEvent { get; set; }

		public int? NurseID { get; set; }
		[ForeignKey("NurseID")]
		public Nurse? Nurse { get; set; }

		[Required]
		[StringLength(100)]
		public string VaccineName { get; set; } = string.Empty;

		public DateTime InjectionDate { get; set; }

		[StringLength(255)]
		public string? Reaction { get; set; }

		[StringLength(50)]
		public string? FollowUpStatus { get; set; }

		[StringLength(50)]
		public string? InjectionSite { get; set; }

		public DateTime? NextDoseDate { get; set; }
	}
} 