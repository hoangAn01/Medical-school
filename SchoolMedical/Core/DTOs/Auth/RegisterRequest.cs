using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.Auth
{
	public class RegisterRequest
	{
		[Required]
		[StringLength(100)]
		public string Username { get; set; } = string.Empty;

		[Required]
		[StringLength(255, MinimumLength = 6)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*(),.?"":{}|<>]).*$",
		ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, and one special character")]
		public string Password { get; set; } = string.Empty;

		[Required]
		[StringLength(100)]
		public string FullName { get; set; } = string.Empty;
		
		[Required]
		public char? Gender { get; set; }
		    
		[Required]
		public DateTime? DateOfBirth { get; set; }

		[StringLength(255)]
		public string? Address { get; set; }
		
		[StringLength(10)]
		public string? Phone { get; set; }
		
		// public string MedicineType { get; set; } = string.Empty;
	}
}