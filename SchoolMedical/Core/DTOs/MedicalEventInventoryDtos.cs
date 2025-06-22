using System;

namespace SchoolMedical.Core.DTOs
{
    public class MedicalEventInventoryDto
    {
        public int EventInventoryID { get; set; }
        public int EventID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime UsedTime { get; set; }
    }

    public class CreateMedicalEventInventoryDto
    {
        public int EventID { get; set; }
        public int ItemID { get; set; }
        public int QuantityUsed { get; set; }
    }

    public class UpdateMedicalEventInventoryDto
    {
        public int EventID { get; set; }
        public int ItemID { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime UsedTime { get; set; }
    }
} 