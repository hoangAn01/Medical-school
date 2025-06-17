using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Infrastructure.Data;

namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class NurseController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public NurseController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Nurse?userId=1
		[HttpGet] // [HttpGet("{id}")]
		public async Task<ActionResult<NurseDTO>> GetNurse([FromQuery] int? userId = null)
		{
			var query = _context.Nurses
				.Include(n => n.Account)
				.AsQueryable();

			if (userId.HasValue)
			{
				// Verify account exists
				var accountExists = await _context.Accounts.AnyAsync(a => a.UserID == userId);
				if (!accountExists)
					return NotFound($"Account with UserID {userId} not found");

				query = query.Where(n => n.UserID == userId);
			}

			var nurse = await query
				.Select(n => new NurseDTO
				{
					NurseID = n.NurseID,
					FullName = n.FullName,
					Gender = n.Gender,
					DateOfBirth = n.DateOfBirth,
					Phone = n.Phone,
					UserID = n.UserID
				})
				.FirstOrDefaultAsync();

			if (nurse == null)
				return NotFound("Nurse not found");

			return nurse;
		}
	}
}