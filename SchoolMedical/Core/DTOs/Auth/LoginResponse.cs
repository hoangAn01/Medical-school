namespace SchoolMedical.Core.DTOs.Auth
{
	public class LoginResponse
	{
		public bool Success { get; set; }
		public string Message { get; set; } = string.Empty;
		public string? Token { get; set; }
		public UserInfo? User { get; set; }
	}

	public class UserInfo
	{
		public int UserID { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
		public string? FullName { get; set; }
	}
}