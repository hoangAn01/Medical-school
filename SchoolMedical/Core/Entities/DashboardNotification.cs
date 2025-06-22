using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("DashboardNotification")]
    public class DashboardNotification
    {
        [Key]
        public int NotificationID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Type { get; set; }

        public int Priority { get; set; }

        public bool IsRead { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
    }
} 