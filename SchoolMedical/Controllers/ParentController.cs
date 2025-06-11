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
	}
}