using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs;
using SchoolMedical.Core.Entities;
using SchoolMedical.Infrastructure.Data;
using BCrypt.Net;

namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/admin/accounts")]
	// [Authorize(Roles = "ManagerAdmin")]
	public class AdminAccountController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public AdminAccountController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/admin/accounts?search=...&role=...&page=1&pageSize=20
		[HttpGet]
		public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts(string? search = null, string? role = null, int page = 1, int pageSize = 20)
		{
			var query = _context.Accounts.AsQueryable();
			if (!string.IsNullOrEmpty(search))
				query = query.Where(a => a.Username.Contains(search));
			if (!string.IsNullOrEmpty(role))
				query = query.Where(a => a.Role == role);
			var total = await query.CountAsync();
			var accounts = await query
				.OrderBy(a => a.UserID)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(a => new AccountDTO { UserID = a.UserID, Username = a.Username, Role = a.Role })
				.ToListAsync();
			return Ok(new { total, accounts });
		}

		// POST: api/admin/accounts
		[HttpPost]
		public async Task<ActionResult<AccountDTO>> CreateAccount([FromBody] AccountCreateRequest request)
		{
			if (await _context.Accounts.AnyAsync(a => a.Username == request.Username))
				return BadRequest(new { message = "Username already exists" });
			var account = new Account
			{
				Username = request.Username,
				PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
				Role = request.Role
			};
			_context.Accounts.Add(account);
			await _context.SaveChangesAsync();
			return Ok(new AccountDTO { UserID = account.UserID, Username = account.Username, Role = account.Role });
		}

		// PUT: api/admin/accounts/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult<AccountDTO>> UpdateAccount(int id, [FromBody] AccountUpdateRequest request)
		{
			var account = await _context.Accounts.FindAsync(id);
			if (account == null)
				return NotFound();
			if (!string.IsNullOrEmpty(request.Password))
				account.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
			if (!string.IsNullOrEmpty(request.Role))
				account.Role = request.Role;
			await _context.SaveChangesAsync();
			return Ok(new AccountDTO { UserID = account.UserID, Username = account.Username, Role = account.Role });
		}

		// DELETE: api/admin/accounts/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAccount(int id)
		{
			var account = await _context.Accounts.FindAsync(id);
			if (account == null)
				return NotFound();
			_context.Accounts.Remove(account);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}
