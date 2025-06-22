using System;
using System.Collections.Generic;

namespace SchoolMedical.Core.DTOs
{
    public class DashboardOverviewDTO
    {
        public int TotalStudents { get; set; }
        public int ActiveCases { get; set; }
        public int NewCases { get; set; }
        public int HealthCheckups { get; set; }
        public int MedicineRequests { get; set; }
        public decimal VaccinationRate { get; set; }
        public List<DashboardNotificationDTO> Notifications { get; set; } = new List<DashboardNotificationDTO>();
    }

    public class DashboardTrendDTO
    {
        public string Period { get; set; }
        public int MedicalEvents { get; set; }
        public int Checkups { get; set; }
        public int NewVaccinations { get; set; }
    }

    public class DashboardNotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DashboardPreferencesDTO
    {
        public string Widgets { get; set; }
        public string Theme { get; set; }
        public int RefreshInterval { get; set; }
    }
}
