using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("Account")]
    public class Account
    {
        [Key]
		// [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;

        // Remove navigation properties to avoid confusion
        // Entity Framework will handle relationships through explicit configuration
    }
}
