using System;

namespace SchoolMedical.Core.DTOs
{
	public class NurseDTO
	{
		public int NurseID { get; set; }
		public string? FullName { get; set; }
		public char? Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Phone { get; set; }
		public int? UserID { get; set; }
	}
}