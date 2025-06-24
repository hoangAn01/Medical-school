using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    [Table("MedicalEventInventory")]
    public class MedicalEventInventory
    {
        [Key]
        public int EventInventoryID { get; set; }
        public int EventID { get; set; }
        public int ItemID { get; set; }
        public int QuantityUsed { get; set; }
        public DateTime UsedTime { get; set; }
    }
}