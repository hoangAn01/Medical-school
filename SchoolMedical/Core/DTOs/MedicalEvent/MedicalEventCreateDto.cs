namespace SchoolMedical.Core.DTOs.MedicalEvent
{
    public class MedicalEventCreateDto
    {
        public int StudentID { get; set; }
        public string EventType { get; set; }
        public string Description { get; set; }
        public DateTime EventTime { get; set; }
        public int? NurseID { get; set; }
    }
} 