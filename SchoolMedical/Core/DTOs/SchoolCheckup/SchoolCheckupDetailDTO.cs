using System;

namespace SchoolMedical.Core.DTOs.SchoolCheckup
{
    public class SchoolCheckupDetailDTO
    {
        public int CheckupID { get; set; }
        public DateTime Date { get; set; }
        public string StudentName { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string BloodPressure { get; set; }
        public string VisionLeft { get; set; }
        public string VisionRight { get; set; }
        public string Description { get; set; }
        public string NurseName { get; set; }
    }
} 