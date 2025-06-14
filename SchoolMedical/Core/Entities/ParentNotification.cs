using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("ParentNotification")]
    public class ParentNotification
    {
        [Key]
        [Column(Order = 0)]
        public int NotificationID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int ParentID { get; set; }

        public DateTime IndividualSentDate { get; set; }

        [StringLength(50)]
        public string? IndividualStatus { get; set; }

        // Navigation properties
        [ForeignKey("NotificationID")]
        public Notification Notification { get; set; }

        [ForeignKey("ParentID")]
        public Parent Parent { get; set; }
    }
} 