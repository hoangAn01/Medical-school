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
		public string Role { get; set; } = "User";
	}
}