using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("SchoolCheckup")]
    public class SchoolCheckup
    {
        [Key]
        public int CheckupID { get; set; }
        public int ReportID { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }
        
        [Column(TypeName = "decimal(5,2)")]
        public decimal? Height { get; set; }
        
        [StringLength(20)]
        public string BloodPressure { get; set; }
        
        [StringLength(10)]
        public string VisionLeft { get; set; }
        
        [StringLength(10)]
        public string VisionRight { get; set; }
        
        // Navigation properties
        [ForeignKey("ReportID")]
        public virtual HealthReport HealthReport { get; set; }
    }
}
