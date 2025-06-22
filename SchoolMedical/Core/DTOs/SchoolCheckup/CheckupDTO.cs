using System;

namespace SchoolMedical.Core.DTOs.SchoolCheckup
{
    public class CheckupDTO
    {
        public int CheckupID { get; set; }
        public int ReportID { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string BloodPressure { get; set; }
        public string VisionLeft { get; set; }
        public string VisionRight { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int? NurseID { get; set; }
        public string NurseName { get; set; }
    }
} 