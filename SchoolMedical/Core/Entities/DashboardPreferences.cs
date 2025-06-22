using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("DashboardPreferences")]
    public class DashboardPreferences
    {
        [Key]
        public int Id { get; set; }

        public int UserID { get; set; }

        public string? PreferredWidgets { get; set; }

        public string? Theme { get; set; }

        public int RefreshInterval { get; set; }

        // Navigation property to User/Account if needed
        // public virtual Account Account { get; set; }
    }
} 