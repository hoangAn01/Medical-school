namespace SchoolMedical.Core.DTOs
{
	public class StudentDTO
	{
		public int StudentID { get; set; }
		public string? FullName { get; set; }
		public char? Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public int? ParentID { get; set; }
		public int? UserID { get; set; }
		public int? ClassID { get; set; }
		public string? ParentName { get; set; }
		public string? ClassName { get; set; }
	}
}