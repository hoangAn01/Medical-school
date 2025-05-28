using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("Account")]
    public class Account
    {
        [Key]
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

        // Navigation properties
        public virtual ManagerAdmin? ManagerAdmin { get; set; }
        public virtual Nurse? Nurse { get; set; }
        public virtual Parent? Parent { get; set; }
        public virtual Student? Student { get; set; }
    }
}