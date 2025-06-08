namespace SchoolMedical.Core.DTOs
{
	public class AccountDTO
	{
		public int UserID { get; set; }
		public string Username { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
	}

	public class AccountCreateRequest
	{
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
	}

	public class AccountUpdateRequest
	{
		public string? Password { get; set; }
		public string? Role { get; set; }
	}
}
