using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{[Table("ParentalConsent")]
    public class ParentalConsent
    {
        
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsentID { get; set; }

        public int StudentID { get; set; }
        [ForeignKey("StudentID")]
        public Student Student { get; set; }

        public int VaccinationEventID { get; set; }
        [ForeignKey("VaccinationEventID")]
        public VaccinationEvent VaccinationEvent { get; set; }

        public int ParentID { get; set; }
        [ForeignKey("ParentID")]
        public Parent Parent { get; set; }

        [Required]
        [StringLength(50)]
        public string ConsentStatus { get; set; } // Ví dụ: 'Chờ phản hồi', 'Đã đồng ý', 'Từ chối'

        public DateTime? ConsentDate { get; set; }

        [StringLength(255)]
        public string? Note { get; set; }
    }
} 