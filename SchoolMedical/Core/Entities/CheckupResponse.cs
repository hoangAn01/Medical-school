using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("CheckupResponse")]
    public class CheckupResponse
    {
        [Key]
        [Column(Order = 0)]
        public int CheckupID { get; set; }
        
        [Key]
        [Column(Order = 1)]
        public int ParentID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string ResponseStatus { get; set; } // "Đã đồng ý", "Từ chối", "Chờ phản hồi"
        
        public DateTime? ResponseDate { get; set; }
        
        [StringLength(255)]
        public string Note { get; set; }
        
        // Navigation properties
        [ForeignKey("CheckupID")]
        public virtual SchoolCheckup Checkup { get; set; }
        
        [ForeignKey("ParentID")]
        public virtual Parent Parent { get; set; }
    }
} 