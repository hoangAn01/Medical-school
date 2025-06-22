using System;

namespace SchoolMedical.Core.DTOs
{
    public class HealthReportDTO
    {
        public int ReportID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int? NurseID { get; set; }
        public string NurseName { get; set; }
        public bool HasCheckup { get; set; }
    }

    public class CreateHealthReportRequest
    {
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public int StudentID { get; set; }
        public int? NurseID { get; set; }
    }
} 