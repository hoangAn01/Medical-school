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
		public string Password { get; set; } = string.Empty;

		[Required]
		[StringLength(100)]
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 93fa48345c0a3c8f3500ad436c26a0c65bdded0f
		public string FullName { get; set; } = string.Empty;
		
		[Required]
		public char? Gender { get; set; }

		[Required]
		public DateTime? DateOfBirth { get; set; }

		[StringLength(255)]
		public string? Address { get; set; }
		
		[StringLength(20)]
		public string? Phone { get; set; }
<<<<<<< HEAD
=======
=======
		public string Role { get; set; } = "User";
>>>>>>> e4b0a303f915aed42098f95d1cde43130b261ee7
>>>>>>> 93fa48345c0a3c8f3500ad436c26a0c65bdded0f
	}
}