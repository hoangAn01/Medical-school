using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Entity
{
	[Table("Account")]
	public class Account{
		[Key]
		[Required]
		public int UserID { get; private set; }

		[Required]
		[MaxLength(100)]
		public string Username { get; private set; }

		[Required]
		[MaxLength(255)]
		public string PasswordHash { get; private set; }

		[Required]
		[MaxLength(50)]
		public string Role { get; private set;}

		public Account() { } // Kết nối Entity Framework yêu cầu có constructor không tham số

		public Account(int userId, string username, string passwordHash, string role){
			UserID = userId;
			Username = username;
			PasswordHash = passwordHash;
			Role = role;
		}
	}
}