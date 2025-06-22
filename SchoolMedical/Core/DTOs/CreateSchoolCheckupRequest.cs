namespace SchoolMedical.Core.DTOs
{
    public class CreateSchoolCheckupRequest
    {
        public int ReportID { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public string BloodPressure { get; set; }
        public string VisionLeft { get; set; }
        public string VisionRight { get; set; }
    }
} 