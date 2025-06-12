namespace SchoolMedical.Core.DTOs
{
    public class AccountDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    public class AccountCreateRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public bool Active { get; set; } = true;
    }

    public class AccountUpdateRequest
    {
        public string? Password { get; set; }
        public string? Role { get; set; }
        public bool? Active { get; set; }
    }

    public class AccountDTOfullName
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string? FullName { get; set; } = null; // Nullable for roles without full name
        public bool Active { get; set; }
    }
}