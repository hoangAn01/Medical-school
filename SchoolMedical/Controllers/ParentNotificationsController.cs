using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs.ParentNotification;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolMedical.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ParentNotificationsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ParentNotificationsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/ParentNotifications
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ParentNotificationDto>>> GetParentNotifications()
		{
			var parentNotifications = await _context.ParentNotifications
				.Include(pn => pn.Notification)
				.Include(pn => pn.Parent)
				.Select(pn => new ParentNotificationDto
				{
					NotificationID = pn.NotificationID,
					ParentID = pn.ParentID,
					ParentName = pn.Parent.FullName,
					NotificationTitle = pn.Notification.Title,
					IndividualSentDate = pn.IndividualSentDate,
					IndividualStatus = pn.IndividualStatus
				})
				.ToListAsync();

			return Ok(parentNotifications);
		}

		// GET: api/ParentNotifications/notification/{notificationId}/parent/{parentId}
		[HttpGet("notification/{notificationId}/parent/{parentId}")]
		public async Task<ActionResult<ParentNotificationDto>> GetParentNotification(
			int notificationId, int parentId)
		{
			var parentNotification = await _context.ParentNotifications
				.Include(pn => pn.Notification)
				.Include(pn => pn.Parent)
				.Where(pn => pn.NotificationID == notificationId && pn.ParentID == parentId)
				.Select(pn => new ParentNotificationDto
				{
					NotificationID = pn.NotificationID,
					ParentID = pn.ParentID,
					ParentName = pn.Parent.FullName,
					NotificationTitle = pn.Notification.Title,
					IndividualSentDate = pn.IndividualSentDate,
					IndividualStatus = pn.IndividualStatus
				})
				.FirstOrDefaultAsync();

			if (parentNotification == null)
			{
				return NotFound();
			}

			return Ok(parentNotification);
		}

		// POST: api/ParentNotifications
		[HttpPost]
		public async Task<ActionResult<ParentNotificationDto>> CreateParentNotification(ParentNotificationDto createDto)
		{
			// Check if Parent and Notification exist
			var parentExists = await _context.Parents.AnyAsync(p => p.ParentID == createDto.ParentID);
			var notificationExists = await _context.Notifications.AnyAsync(n => n.NotificationID == createDto.NotificationID);

			if (!parentExists || !notificationExists)
			{
				return BadRequest("Invalid ParentID or NotificationID.");
			}

			var parentNotification = new ParentNotification
			{
				NotificationID = createDto.NotificationID,
				ParentID = createDto.ParentID,
				IndividualSentDate = DateTime.Now,
				IndividualStatus = createDto.IndividualStatus ?? "Sent"
			};

			_context.ParentNotifications.Add(parentNotification);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				if (ParentNotificationExists(parentNotification.NotificationID, parentNotification.ParentID))
				{
					return Conflict("This parent notification already exists.");
				}
				else
				{
					throw;
				}
			}

			var createdParentNotification = await _context.ParentNotifications
				.Include(pn => pn.Notification)
				.Include(pn => pn.Parent)
				.Where(pn => pn.NotificationID == parentNotification.NotificationID && pn.ParentID == parentNotification.ParentID)
				.Select(pn => new ParentNotificationDto
				{
					NotificationID = pn.NotificationID,
					ParentID = pn.ParentID,
					ParentName = pn.Parent.FullName,
					NotificationTitle = pn.Notification.Title,
					IndividualSentDate = pn.IndividualSentDate,
					IndividualStatus = pn.IndividualStatus
				})
				.FirstOrDefaultAsync();

			return CreatedAtAction(nameof(GetParentNotification), 
				new { notificationId = createdParentNotification?.NotificationID, parentId = createdParentNotification?.ParentID }, createdParentNotification);
		}

		// PUT: api/ParentNotifications/notification/{notificationId}/parent/{parentId}
		[HttpPut("notification/{notificationId}/parent/{parentId}")]
		public async Task<IActionResult> UpdateParentNotification(
			int notificationId, int parentId, ParentNotificationDto updateDto)
		{
			var parentNotification = await _context.ParentNotifications
				.FindAsync(notificationId, parentId);

			if (parentNotification == null)
			{
				return NotFound();
			}
			
			// Ensure the IDs in the DTO match the route parameters
			if (notificationId != updateDto.NotificationID || parentId != updateDto.ParentID)
			{
				return BadRequest("NotificationID or ParentID in DTO does not match route parameters.");
			}

			parentNotification.IndividualSentDate = updateDto.IndividualSentDate;
			parentNotification.IndividualStatus = updateDto.IndividualStatus ?? parentNotification.IndividualStatus;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ParentNotificationExists(notificationId, parentId))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// DELETE: api/ParentNotifications/notification/{notificationId}/parent/{parentId}
		[HttpDelete("notification/{notificationId}/parent/{parentId}")]
		public async Task<IActionResult> DeleteParentNotification(int notificationId, int parentId)
		{
			var parentNotification = await _context.ParentNotifications
				.FindAsync(notificationId, parentId);
			if (parentNotification == null)
			{
				return NotFound();
			}

			_context.ParentNotifications.Remove(parentNotification);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET: api/ParentNotifications/parent/{parentId}
		[HttpGet("parent/{parentId}")]
		public async Task<ActionResult<IEnumerable<ParentNotificationDto>>> GetNotificationsByParent(int parentId)
		{
			// Validate parent exists
			var parent = await _context.Parents.FindAsync(parentId);
			if (parent == null)
				return BadRequest("Invalid ParentID");

			var parentNotifications = await _context.ParentNotifications
				.Include(pn => pn.Notification)
				.Include(pn => pn.Parent)
				.Where(pn => pn.ParentID == parentId)
				.Select(pn => new ParentNotificationDto
				{
					NotificationID = pn.NotificationID,
					ParentID = pn.ParentID,
					ParentName = pn.Parent.FullName,
					NotificationTitle = pn.Notification.Title,
					IndividualSentDate = pn.IndividualSentDate,
					IndividualStatus = pn.IndividualStatus
				})
				.OrderByDescending(pn => pn.IndividualSentDate)
				.ToListAsync();

			return Ok(parentNotifications);
		}

		// GET: api/ParentNotifications/parent/{parentId}/details
		[HttpGet("parent/{parentId}/details")]
		public async Task<ActionResult<IEnumerable<ParentNotificationDetailDto>>> GetDetailedNotificationsByParent(int parentId)
		{
			// Validate parent exists
			var parent = await _context.Parents.FindAsync(parentId);
			if (parent == null)
				return BadRequest("Invalid ParentID");

			var parentNotifications = await _context.ParentNotifications
				.Include(pn => pn.Notification)
				.Include(pn => pn.Parent)
				.Where(pn => pn.ParentID == parentId)
				.Select(pn => new ParentNotificationDetailDto
				{
					NotificationID = pn.NotificationID,
					ParentID = pn.ParentID,
					ParentName = pn.Parent.FullName,
					NotificationTitle = pn.Notification.Title,
					NotificationContent = pn.Notification.Content,
					IndividualSentDate = pn.IndividualSentDate,
					IndividualStatus = pn.IndividualStatus,
					NotificationSentDate = pn.Notification.SentDate,
					NotificationStatus = pn.Notification.Status
				})
				.OrderByDescending(pn => pn.IndividualSentDate)
				.ToListAsync();

			return Ok(parentNotifications);
		}

		// PUT: api/ParentNotifications/parent/{parentId}/notification/{notificationId}/mark-read
		[HttpPut("parent/{parentId}/notification/{notificationId}/mark-read")]
		public async Task<IActionResult> MarkNotificationAsRead(int parentId, int notificationId)
		{
			var parentNotification = await _context.ParentNotifications
				.FindAsync(notificationId, parentId);

			if (parentNotification == null)
			{
				return NotFound();
			}

			parentNotification.IndividualStatus = "Read";

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!ParentNotificationExists(notificationId, parentId))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// GET: api/ParentNotifications/search?keyword=abc
		[HttpGet("search")]
		public async Task<ActionResult<IEnumerable<ParentNotificationDto>>> SearchParentNotifications(
			[FromQuery] string? keyword)
		{
			var query = _context.ParentNotifications
				.Include(pn => pn.Notification)
				.Include(pn => pn.Parent)
				.AsQueryable();

			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(pn =>
					pn.Parent.FullName.Contains(keyword) ||
					pn.Notification.Title.Contains(keyword) ||
					pn.IndividualStatus.Contains(keyword));
			}

			var parentNotifications = await query
				.Select(pn => new ParentNotificationDto
				{
					NotificationID = pn.NotificationID,
					ParentID = pn.ParentID,
					ParentName = pn.Parent.FullName,
					NotificationTitle = pn.Notification.Title,
					IndividualSentDate = pn.IndividualSentDate,
					IndividualStatus = pn.IndividualStatus
				})
				.ToListAsync();

			return Ok(parentNotifications);
		}

		private bool ParentNotificationExists(int notificationId, int parentId)
		{
			return _context.ParentNotifications.Any(e => e.NotificationID == notificationId && e.ParentID == parentId);
		}
	}
}
