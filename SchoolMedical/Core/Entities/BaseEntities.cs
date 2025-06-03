using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
	[Table("ManagerAdmin")]
	public class ManagerAdmin
	{
		[Key]
		public int ManagerID { get; set; }
		public string? FullName { get; set; }
		public char? Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Address { get; set; }
		public int? UserID { get; set; }

		// Navigation property
		public virtual Account? Account { get; set; }
	}

	[Table("Nurse")]
	public class Nurse
	{
		[Key]
		public int NurseID { get; set; }
		public string? FullName { get; set; }
		public char? Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Phone { get; set; }
		public int? UserID { get; set; }

		// Navigation property
		public virtual Account? Account { get; set; }
	}

	[Table("Parent")]
	public class Parent
	{
		[Key]
		public int ParentID { get; set; }
		public int? UserID { get; set; }
		public string? FullName { get; set; }
		public char? Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Address { get; set; }
		public string? Phone { get; set; }

		// Navigation property
		public virtual Account? Account { get; set; }
	}

	[Table("Student")]
	public class Student
	{
		[Key]
		public int StudentID { get; set; }
		public string? FullName { get; set; }
		public char? Gender { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public int? ParentID { get; set; }
		public int? UserID { get; set; }
		public int? ClassID { get; set; }

		// Navigation properties
		public virtual Account? Account { get; set; }
		public virtual Parent? Parent { get; set; }
		public virtual Class? Class { get; set; }
	}

	[Table("Class")]
	public class Class
	{
		[Key]
		public int ClassID { get; set; }
		public string ClassName { get; set; } = string.Empty;
		public string? SchoolYear { get; set; }
		public string? TeacherName { get; set; }
		public string? Description { get; set; }
		public int? TeacherID { get; set; }

		// Navigation properties
		public virtual ICollection<Student> Students { get; set; } = new List<Student>();
	}

	[Table("HealthProfile")]
	public class HealthProfile
	{
		[Key]
		public int ProfileID { get; set; }

		public int? StudentID { get; set; }
		public string? ChronicDisease { get; set; }
		public string? VisionTest { get; set; }
		public string? Allergy { get; set; }
		public decimal? Weight { get; set; }
		public decimal? Height { get; set; }
		public DateTime? LastCheckupDate { get; set; }

		// Navigation property
		public virtual Student? Student { get; set; }
	}
}
