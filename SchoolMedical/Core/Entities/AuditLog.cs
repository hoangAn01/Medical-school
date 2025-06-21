using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolMedical.Core.Entities
{
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }
        public string TableName { get; set; }
        public string Action { get; set; }
        public int? UserID { get; set; }
        public DateTime ActionDate { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
