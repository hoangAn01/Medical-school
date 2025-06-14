using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("VaccinationEvent")]
    public class VaccinationEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventID { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        [StringLength(255)]
        public string? Location { get; set; }

        public int? ManagerID { get; set; }
        [ForeignKey("ManagerID")]
        public ManagerAdmin? ManagerAdmin { get; set; }

        public int? ClassID { get; set; }
        [ForeignKey("ClassID")]
        public Class? Class { get; set; }

        // Navigation property for VaccineRecords (optional, if you want to navigate from event to records)
        public ICollection<VaccineRecord> VaccineRecords { get; set; } = new List<VaccineRecord>();
    }
} 