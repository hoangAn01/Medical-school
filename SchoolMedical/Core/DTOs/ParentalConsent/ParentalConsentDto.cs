using System;

namespace SchoolMedical.Core.DTOs.ParentalConsent
{
    public class ParentalConsentDto
    {
        public int ConsentID { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; } // For displaying student name
        public int VaccinationEventID { get; set; }
        public string EventName { get; set; } // For displaying vaccination event name
        public int ParentID { get; set; }
        public string ParentName { get; set; } // For displaying parent name
        public string ConsentStatus { get; set; }
        public DateTime? ConsentDate { get; set; }
        public string? Note { get; set; }
    }
} 