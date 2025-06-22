using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("HealthReport")]
    public class HealthReport
    {
        [Key]
        public int ReportID { get; set; }
        
        public DateTime Date { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
        
        public int StudentID { get; set; }
        
        public int? NurseID { get; set; }
        
        // Navigation properties
        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }
        
        [ForeignKey("NurseID")]
        public virtual Nurse Nurse { get; set; }
        
        public virtual SchoolCheckup SchoolCheckup { get; set; }
    }
} 