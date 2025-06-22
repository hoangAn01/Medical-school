using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolMedical.Services
{
    public interface IDashboardService
    {
        Task<DashboardOverviewDTO> GetDashboardOverview();
        Task<List<DashboardTrendDTO>> GetTrends(DateTime? startDate, DateTime? endDate);
        Task<List<DashboardNotificationDTO>> GetNotifications();
        Task<DashboardPreferencesDTO> GetUserPreferences();
        Task UpdatePreferences(DashboardPreferencesDTO preferences);
    }

    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly int _userId; // Get from authentication context

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
            // _userId = GetUserIdFromContext();
        }

        public async Task<DashboardOverviewDTO> GetDashboardOverview()
        {
            var today = DateTime.Now.Date;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var overview = new DashboardOverviewDTO
            {
                TotalStudents = await _context.Students.CountAsync(),
                ActiveCases = await _context.MedicalEvents
                    .Where(e => e.Status == "Active" && e.EventTime >= startOfWeek)
                    .CountAsync(),
                NewCases = await _context.MedicalEvents
                    .Where(e => e.EventTime >= startOfWeek)
                    .CountAsync(),
                HealthCheckups = await _context.HealthProfiles
                    .Where(c => c.LastCheckupDate >= startOfMonth)
                    .CountAsync(),
                MedicineRequests = await _context.MedicineRequests
                    .Where(r => r.RequestStatus == "Pending")
                    .CountAsync(),
                VaccinationRate = await CalculateVaccinationRate(),
                Notifications = await GetNotifications()
            };

            return overview;
        }

        public async Task<List<DashboardTrendDTO>> GetTrends(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Now.AddMonths(-3);
            endDate ??= DateTime.Now;

            var trends = new List<DashboardTrendDTO>();

            // Calculate weekly trends
            var currentWeek = startDate.Value;
            while (currentWeek <= endDate)
            {
                var nextWeek = currentWeek.AddDays(7);
                var weekData = new DashboardTrendDTO
                {
                    Period = $"{currentWeek:dd/MM} - {nextWeek.AddDays(-1):dd/MM}",
                    MedicalEvents = await _context.MedicalEvents
                        .Where(e => e.EventTime >= currentWeek && e.EventTime < nextWeek)
                        .CountAsync(),
                    Checkups = await _context.HealthProfiles
                        .Where(c => c.LastCheckupDate >= currentWeek && c.LastCheckupDate < nextWeek)
                        .CountAsync(),
                    NewVaccinations = await _context.VaccineRecords
                        .Where(v => v.InjectionDate >= currentWeek && v.InjectionDate < nextWeek)
                        .CountAsync()
                };
                trends.Add(weekData);
                currentWeek = nextWeek;
            }

            return trends;
        }

        public async Task<List<DashboardNotificationDTO>> GetNotifications()
        {
            var notifications = await _context.DashboardNotifications
                .Where(n => n.UserID == _userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedDate)
                .Select(n => new DashboardNotificationDTO
                {
                    Id = n.NotificationID,
                    Title = n.Title,
                    Message = n.Message,
                    Type = n.Type,
                    Priority = n.Priority,
                    CreatedDate = n.CreatedDate
                })
                .ToListAsync();

            return notifications;
        }

        public async Task<DashboardPreferencesDTO> GetUserPreferences()
        {
            var preferences = await _context.DashboardPreferences
                .Where(p => p.UserID == _userId)
                .Select(p => new DashboardPreferencesDTO
                {
                    Widgets = p.PreferredWidgets,
                    Theme = p.Theme,
                    RefreshInterval = p.RefreshInterval
                })
                .FirstOrDefaultAsync();

            return preferences ?? new DashboardPreferencesDTO();
        }

        public async Task UpdatePreferences(DashboardPreferencesDTO preferences)
        {
            var existing = await _context.DashboardPreferences
                .FirstOrDefaultAsync(p => p.UserID == _userId);

            if (existing == null)
            {
                _context.DashboardPreferences.Add(new Core.Entities.DashboardPreferences
                {
                    UserID = _userId,
                    PreferredWidgets = preferences.Widgets,
                    Theme = preferences.Theme,
                    RefreshInterval = preferences.RefreshInterval
                });
            }
            else
            {
                existing.PreferredWidgets = preferences.Widgets;
                existing.Theme = preferences.Theme;
                existing.RefreshInterval = preferences.RefreshInterval;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<decimal> CalculateVaccinationRate()
        {
            var totalStudents = await _context.Students.CountAsync();
            var vaccinatedStudents = await _context.VaccineRecords
                .GroupBy(v => v.StudentID)
                .CountAsync();

            return totalStudents > 0 ? (decimal)vaccinatedStudents / totalStudents * 100 : 0;
        }
    }
}
