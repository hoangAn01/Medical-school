using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Infrastructure.Data;

namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ParentController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ParentController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Parent/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<ParentDTO>> GetParent(int id)
		{
			var parent = await _context.Parents
				.FirstOrDefaultAsync(p => p.ParentID == id);

			if (parent == null)
				return NotFound();

			var dto = new ParentDTO
			{
				ParentID = parent.ParentID,
				UserID = parent.UserID,
				FullName = parent.FullName,
				Gender = parent.Gender,
				DateOfBirth = parent.DateOfBirth,
				Address = parent.Address,
				Phone = parent.Phone
			};

			return dto;
		}

		// GET: api/Parent/user/{userId}
		[HttpGet("user/{userId}")]
		public async Task<ActionResult<ParentDTO>> GetParentByUserId(int userId)
		{
			var parent = await _context.Parents
				.FirstOrDefaultAsync(p => p.UserID == userId);

			if (parent == null)
				return NotFound();

			var dto = new ParentDTO
			{
				ParentID = parent.ParentID,
				UserID = parent.UserID,
				FullName = parent.FullName,
				Gender = parent.Gender,
				DateOfBirth = parent.DateOfBirth,
				Address = parent.Address,
				Phone = parent.Phone
			};

			return dto;
		}

		// PUT: api/Parent/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateParent(int id, ParentDTO parentDTO)
		{
			if (id != parentDTO.ParentID)
				return BadRequest();

			var parent = await _context.Parents.FindAsync(id);

			if (parent == null)
				return NotFound();

			// Update parent properties
			parent.FullName = parentDTO.FullName;
			parent.Gender = parentDTO.Gender;
			parent.DateOfBirth = parentDTO.DateOfBirth;
			parent.Address = parentDTO.Address;
			parent.Phone = parentDTO.Phone;
			// Note: UserID is typically not updated as it's a relationship identifier

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await ParentExists(id))
					return NotFound();
				throw;
			}

			return NoContent();
		}

		// Helper method to check if parent exists
		private async Task<bool> ParentExists(int id)
		{
			return await _context.Parents.AnyAsync(e => e.ParentID == id);
		}
	}
}