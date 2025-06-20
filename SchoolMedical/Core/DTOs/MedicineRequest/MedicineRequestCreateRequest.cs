using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolMedical.Core.DTOs.MedicineRequest
{
    public class MedicineRequestCreateRequest
    {
        [Required]
        public int StudentID { get; set; }
        
        public int? ParentID { get; set; }
        
        [StringLength(255)]
        public string? Note { get; set; }
        
        [Required]
        public List<MedicineRequestDetailCreateRequest> MedicineDetails { get; set; } = new();
    }

    public class MedicineRequestDetailCreateRequest
    {
        [Required]
        public string ItemName { get; set; } = string.Empty;
        
        // public string? MedicineType { get; set; }

        [Required]
        [Range(1, int.MaxValue)] // Changed minimum from 1 to 0
        public int Quantity { get; set; }
        
        public string? DosageInstructions { get; set; }
        public string? Time { get; set; }
    }
}