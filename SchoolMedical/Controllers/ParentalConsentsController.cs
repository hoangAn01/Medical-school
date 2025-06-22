using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs.ParentalConsent;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SchoolMedical.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ParentalConsentsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ParentalConsentsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/ParentalConsents
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ParentalConsentDto>>> GetParentalConsents()
		{
			var consents = await _context.ParentalConsents
				.Include(pc => pc.Student)
				.Include(pc => pc.VaccinationEvent)
				.Include(pc => pc.SchoolCheckup)
				.ThenInclude(sc => sc.HealthReport)
				.Include(pc => pc.Parent)
				.Select(pc => new ParentalConsentDto
				{
					ConsentID = pc.ConsentID,
					StudentID = pc.StudentID,
					StudentName = pc.Student.FullName,
					VaccinationEventID = pc.VaccinationEventID,
					EventName = pc.VaccinationEvent != null ? pc.VaccinationEvent.EventName : null,
					CheckupID = pc.CheckupID,
					CheckupDate = pc.SchoolCheckup != null ? pc.SchoolCheckup.HealthReport.Date : null,
					ParentID = pc.ParentID,
					ParentName = pc.Parent.FullName,
					ConsentStatus = pc.ConsentStatus,
					ConsentDate = pc.ConsentDate,
					Note = pc.Note,
					ConsentType = pc.VaccinationEventID.HasValue ? "Vaccination" : "Checkup"
				})
				.ToListAsync();

			return Ok(consents);
		}

		// GET: api/ParentalConsents/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ParentalConsentDto>> GetParentalConsent(int id)
		{
			var consent = await _context.ParentalConsents
				.Include(pc => pc.Student)
				.Include(pc => pc.VaccinationEvent)
				.Include(pc => pc.SchoolCheckup)
				.ThenInclude(sc => sc.HealthReport)
				.Include(pc => pc.Parent)
				.Where(pc => pc.ConsentID == id)
				.Select(pc => new ParentalConsentDto
				{
					ConsentID = pc.ConsentID,
					StudentID = pc.StudentID,
					StudentName = pc.Student.FullName,
					VaccinationEventID = pc.VaccinationEventID,
					EventName = pc.VaccinationEvent != null ? pc.VaccinationEvent.EventName : null,
					CheckupID = pc.CheckupID,
					CheckupDate = pc.SchoolCheckup != null ? pc.SchoolCheckup.HealthReport.Date : null,
					ParentID = pc.ParentID,
					ParentName = pc.Parent.FullName,
					ConsentStatus = pc.ConsentStatus,
					ConsentDate = pc.ConsentDate,
					Note = pc.Note,
					ConsentType = pc.VaccinationEventID.HasValue ? "Vaccination" : "Checkup"
				})
				.FirstOrDefaultAsync();

			if (consent == null)
			{
				return NotFound();
			}

			return Ok(consent);
		}

		// POST: api/ParentalConsents
		[HttpPost]
		public async Task<ActionResult<ParentalConsentDto>> CreateParentalConsent(ParentalConsentCreateDto createDto)
		{
			// Validate input: Phải có một trong hai: VaccinationEventID hoặc CheckupID
			if (!createDto.VaccinationEventID.HasValue && !createDto.CheckupID.HasValue)
			{
				return BadRequest("Phải cung cấp một trong hai: VaccinationEventID hoặc CheckupID");
			}

			// Không được cung cấp cả hai
			if (createDto.VaccinationEventID.HasValue && createDto.CheckupID.HasValue)
			{
				return BadRequest("Chỉ được cung cấp một trong hai: VaccinationEventID hoặc CheckupID");
			}

			var parentalConsent = new ParentalConsent
			{
				StudentID = createDto.StudentID,
				VaccinationEventID = createDto.VaccinationEventID,
				CheckupID = createDto.CheckupID,
				ParentID = createDto.ParentID,
				ConsentStatus = createDto.ConsentStatus,
				ConsentDate = createDto.ConsentDate ?? DateTime.Now,
				Note = createDto.Note
			};

			_context.ParentalConsents.Add(parentalConsent);
			await _context.SaveChangesAsync();

			var createdConsent = await _context.ParentalConsents
				.Include(pc => pc.Student)
				.Include(pc => pc.VaccinationEvent)
				.Include(pc => pc.SchoolCheckup)
				.ThenInclude(sc => sc.HealthReport)
				.Include(pc => pc.Parent)
				.Where(pc => pc.ConsentID == parentalConsent.ConsentID)
				.Select(pc => new ParentalConsentDto
				{
					ConsentID = pc.ConsentID,
					StudentID = pc.StudentID,
					StudentName = pc.Student.FullName,
					VaccinationEventID = pc.VaccinationEventID,
					EventName = pc.VaccinationEvent != null ? pc.VaccinationEvent.EventName : null,
					CheckupID = pc.CheckupID,
					CheckupDate = pc.SchoolCheckup != null ? pc.SchoolCheckup.HealthReport.Date : null,
					ParentID = pc.ParentID,
					ParentName = pc.Parent.FullName,
					ConsentStatus = pc.ConsentStatus,
					ConsentDate = pc.ConsentDate,
					Note = pc.Note,
					ConsentType = pc.VaccinationEventID.HasValue ? "Vaccination" : "Checkup"
				})
				.FirstOrDefaultAsync();

			return CreatedAtAction(nameof(GetParentalConsent), new { id = createdConsent?.ConsentID }, createdConsent);
		}

		// PUT: api/ParentalConsents/5
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateParentalConsent(int id, ParentalConsentCreateDto updateDto)
		{
			var parentalConsent = await _context.ParentalConsents.FindAsync(id);
			if (parentalConsent == null)
			{
				return NotFound();
			}

			// Validate input: Phải có một trong hai: VaccinationEventID hoặc CheckupID
			if (!updateDto.VaccinationEventID.HasValue && !updateDto.CheckupID.HasValue)
			{
				return BadRequest("Phải cung cấp một trong hai: VaccinationEventID hoặc CheckupID");
			}

			// Không được cung cấp cả hai
			if (updateDto.VaccinationEventID.HasValue && updateDto.CheckupID.HasValue)
			{
				return BadRequest("Chỉ được cung cấp một trong hai: VaccinationEventID hoặc CheckupID");
			}

			parentalConsent.StudentID = updateDto.StudentID;
			parentalConsent.VaccinationEventID = updateDto.VaccinationEventID;
			parentalConsent.CheckupID = updateDto.CheckupID;
			parentalConsent.ParentID = updateDto.ParentID;
			parentalConsent.ConsentStatus = updateDto.ConsentStatus;
			parentalConsent.ConsentDate = updateDto.ConsentDate ?? DateTime.Now;
			parentalConsent.Note = updateDto.Note;

			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/ParentalConsents/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteParentalConsent(int id)
		{
			var parentalConsent = await _context.ParentalConsents.FindAsync(id);
			if (parentalConsent == null)
			{
				return NotFound();
			}

			_context.ParentalConsents.Remove(parentalConsent);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET: api/ParentalConsents/student/{studentId}/event/{eventId}
		[HttpGet("student/{studentId}/event/{eventId}")]
		public async Task<ActionResult<ParentalConsentDto>> GetParentalConsentByStudentAndEvent(
			int studentId, int eventId)
		{
			var consent = await _context.ParentalConsents
				.Include(pc => pc.Student)
				.Include(pc => pc.VaccinationEvent)
				.Include(pc => pc.Parent)
				.Where(pc => pc.StudentID == studentId && pc.VaccinationEventID == eventId)
				.Select(pc => new ParentalConsentDto
				{
					ConsentID = pc.ConsentID,
					StudentID = pc.StudentID,
					StudentName = pc.Student.FullName,
					VaccinationEventID = pc.VaccinationEventID,
					EventName = pc.VaccinationEvent != null ? pc.VaccinationEvent.EventName : null,
					CheckupID = pc.CheckupID,
					ParentID = pc.ParentID,
					ParentName = pc.Parent.FullName,
					ConsentStatus = pc.ConsentStatus,
					ConsentDate = pc.ConsentDate,
					Note = pc.Note,
					ConsentType = "Vaccination"
				})
				.FirstOrDefaultAsync();

			if (consent == null)
			{
				return NotFound();
			}

			return Ok(consent);
		}

		// GET: api/ParentalConsents/student/{studentId}/checkup/{checkupId}
		[HttpGet("student/{studentId}/checkup/{checkupId}")]
		public async Task<ActionResult<ParentalConsentDto>> GetParentalConsentByStudentAndCheckup(
			int studentId, int checkupId)
		{
			var consent = await _context.ParentalConsents
				.Include(pc => pc.Student)
				.Include(pc => pc.SchoolCheckup)
				.ThenInclude(sc => sc.HealthReport)
				.Include(pc => pc.Parent)
				.Where(pc => pc.StudentID == studentId && pc.CheckupID == checkupId)
				.Select(pc => new ParentalConsentDto
				{
					ConsentID = pc.ConsentID,
					StudentID = pc.StudentID,
					StudentName = pc.Student.FullName,
					CheckupID = pc.CheckupID,
					CheckupDate = pc.SchoolCheckup != null ? pc.SchoolCheckup.HealthReport.Date : null,
					ParentID = pc.ParentID,
					ParentName = pc.Parent.FullName,
					ConsentStatus = pc.ConsentStatus,
					ConsentDate = pc.ConsentDate,
					Note = pc.Note,
					ConsentType = "Checkup"
				})
				.FirstOrDefaultAsync();

			if (consent == null)
			{
				return NotFound();
			}

			return Ok(consent);
		}

		// GET: api/ParentalConsents/search?keyword=abc
		[HttpGet("search")]
		public async Task<ActionResult<IEnumerable<ParentalConsentDto>>> SearchParentalConsents(
			[FromQuery] string? keyword)
		{
			var query = _context.ParentalConsents
				.Include(pc => pc.Student)
				.Include(pc => pc.VaccinationEvent)
				.Include(pc => pc.SchoolCheckup)
				.ThenInclude(sc => sc.HealthReport)
				.Include(pc => pc.Parent)
				.AsQueryable();

			if (!string.IsNullOrEmpty(keyword))
			{
				query = query.Where(pc =>
					pc.Student.FullName.Contains(keyword) ||
					(pc.VaccinationEvent != null && pc.VaccinationEvent.EventName.Contains(keyword)) ||
					pc.Parent.FullName.Contains(keyword) ||
					pc.ConsentStatus.Contains(keyword) ||
					(pc.Note != null && pc.Note.Contains(keyword)));
			}

			var consents = await query
				.Select(pc => new ParentalConsentDto
				{
					ConsentID = pc.ConsentID,
					StudentID = pc.StudentID,
					StudentName = pc.Student.FullName,
					VaccinationEventID = pc.VaccinationEventID,
					EventName = pc.VaccinationEvent != null ? pc.VaccinationEvent.EventName : null,
					CheckupID = pc.CheckupID,
					CheckupDate = pc.SchoolCheckup != null ? pc.SchoolCheckup.HealthReport.Date : null,
					ParentID = pc.ParentID,
					ParentName = pc.Parent.FullName,
					ConsentStatus = pc.ConsentStatus,
					ConsentDate = pc.ConsentDate,
					Note = pc.Note,
					ConsentType = pc.VaccinationEventID.HasValue ? "Vaccination" : "Checkup"
				})
				.ToListAsync();

			return Ok(consents);
		}
	}
}
