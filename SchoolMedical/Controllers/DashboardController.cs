using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using SchoolMedical.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SchoolMedical.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDashboardService _dashboardService;

        public DashboardController(ApplicationDbContext context, IDashboardService dashboardService)
        {
            _context = context;
            _dashboardService = dashboardService;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetDashboardOverview()
        {
            var dashboardData = await _dashboardService.GetDashboardOverview();
            return Ok(dashboardData);
        }

        [HttpGet("trends")]
        public async Task<IActionResult> GetTrends([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            var trends = await _dashboardService.GetTrends(startDate, endDate);
            return Ok(trends);
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _dashboardService.GetNotifications();
            return Ok(notifications);
        }

        [HttpGet("preferences")]
        public async Task<IActionResult> GetUserPreferences()
        {
            var preferences = await _dashboardService.GetUserPreferences();
            return Ok(preferences);
        }

        [HttpPut("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] DashboardPreferencesDTO preferences)
        {
            await _dashboardService.UpdatePreferences(preferences);
            return NoContent();
        }

        [HttpGet("widgets")]
        public IActionResult GetAvailableWidgets()
        {
            var widgets = new[]
            {
                new { Id = "health-stats", Name = "Health Statistics", Description = "Shows key health metrics" },
                new { Id = "active-cases", Name = "Active Cases", Description = "Current active medical cases" },
                new { Id = "vaccination", Name = "Vaccination Status", Description = "Vaccination coverage and trends" },
                new { Id = "checkups", Name = "Health Checkups", Description = "Schedule and status of health checkups" },
                new { Id = "medicine", Name = "Medicine Requests", Description = "Pending and completed medicine requests" }
            };
            return Ok(widgets);
        }
    }
}
