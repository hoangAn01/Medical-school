using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs.Notification;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolMedical.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class NotificationsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public NotificationsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Notifications
		[HttpGet]
		public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
		{
			var notifications = await _context.Notifications
				.Select(n => new NotificationDto
				{
					NotificationID = n.NotificationID,
					Title = n.Title,
					Content = n.Content,
					SentDate = n.SentDate,
					Status = n.Status,
					NotificationType = n.NotificationType,
					VaccinationEventID = n.VaccinationEventID,
					MedicalEventID = n.MedicalEventID
				})
				.ToListAsync();

			return Ok(notifications);
		}

		// GET: api/Notifications/5
		[HttpGet("{id}")]
		public async Task<ActionResult<NotificationDto>> GetNotification(int id)
		{
			var notification = await _context.Notifications
				.Where(n => n.NotificationID == id)
				.Select(n => new NotificationDto
				{
					NotificationID = n.NotificationID,
					Title = n.Title,
					Content = n.Content,
					SentDate = n.SentDate,
					Status = n.Status,
					NotificationType = n.NotificationType,
					VaccinationEventID = n.VaccinationEventID,
					MedicalEventID = n.MedicalEventID
				})
				.FirstOrDefaultAsync();

			if (notification == null)
			{
				return NotFound();
			}

			return Ok(notification);
		}

		// POST: api/Notifications
		[HttpPost]
		public async Task<ActionResult<NotificationDto>> CreateNotification(NotificationCreateDto createDto)
		{
			var notification = new Notification
			{
				Title = createDto.Title,
				Content = createDto.Content,
				SentDate = DateTime.Now, // Set current date/time
				Status = createDto.Status ?? "Draft", // Default status
				NotificationType = createDto.NotificationType,
				VaccinationEventID = createDto.VaccinationEventID,
				MedicalEventID = createDto.MedicalEventID
			};

			_context.Notifications.Add(notification);
			await _context.SaveChangesAsync();

			var createdNotification = await _context.Notifications
				.Where(n => n.NotificationID == notification.NotificationID)
				.Select(n => new NotificationDto
				{
					NotificationID = n.NotificationID,
					Title = n.Title,
					Content = n.Content,
					SentDate = n.SentDate,
					Status = n.Status,
					NotificationType = n.NotificationType,
					VaccinationEventID = n.VaccinationEventID,
					MedicalEventID = n.MedicalEventID
				})
				.FirstOrDefaultAsync();

			return CreatedAtAction(nameof(GetNotification), new { id = createdNotification?.NotificationID }, createdNotification);
		}

		// PUT: api/Notifications/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateNotification(int id, NotificationCreateDto updateDto)
		{
			var notification = await _context.Notifications.FindAsync(id);
			if (notification == null)
			{
				return NotFound();
			}

			notification.Title = updateDto.Title;
			notification.Content = updateDto.Content;
			notification.Status = updateDto.Status ?? notification.Status; // Update status, keep old if null
			notification.NotificationType = updateDto.NotificationType ?? notification.NotificationType; // Update type, keep old if null
			notification.VaccinationEventID = updateDto.VaccinationEventID;
			notification.MedicalEventID = updateDto.MedicalEventID;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/Notifications/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteNotification(int id)
		{
			var notification = await _context.Notifications.FindAsync(id);
			if (notification == null)
			{
				return NotFound();
			}

			_context.Notifications.Remove(notification);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET: api/Notifications/search?keyword=abc
		[HttpGet("search")]
		public async Task<ActionResult<IEnumerable<NotificationDto>>> SearchNotifications(
			[FromQuery] string? keyword)
		{
			var query = _context.Notifications.AsQueryable();

			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(n =>
					n.Title.Contains(keyword) ||
					n.Content.Contains(keyword) ||
					n.NotificationType.Contains(keyword));
			}

			var notifications = await query
				.Select(n => new NotificationDto
				{
					NotificationID = n.NotificationID,
					Title = n.Title,
					Content = n.Content,
					SentDate = n.SentDate,
					Status = n.Status,
					NotificationType = n.NotificationType,
					VaccinationEventID = n.VaccinationEventID,
					MedicalEventID = n.MedicalEventID
				})
				.ToListAsync();

			return Ok(notifications);
		}
	}
}