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
	[Authorize(Roles = "Admin")] // Comnent this line if you want to allow all roles to access this controller
	public class AdminAccountController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public AdminAccountController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/admin/accounts/all
		[HttpGet("all")]
		public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAllAccounts()
		{
			var accounts = await (from a in _context.Accounts
								  join p in _context.Parents on a.UserID equals p.UserID into parentJoin
								  from parent in parentJoin.DefaultIfEmpty()
								  join n in _context.Nurses on a.UserID equals n.UserID into nurseJoin
								  from nurse in nurseJoin.DefaultIfEmpty()
								  join m in _context.ManagerAdmins on a.UserID equals m.UserID into managerJoin
								  from manager in managerJoin.DefaultIfEmpty()
								  orderby a.UserID
								  select new AccountDTOfullName
								  {
									  UserID = a.UserID,
									  Username = a.Username,
									  Role = a.Role,
									  Active = a.Active,
									  FullName = a.Role == "Parent" ? parent.FullName :
											   a.Role == "Nurse" ? nurse.FullName :
											   a.Role == "Admin" ? manager.FullName :
											   null
								  }).ToListAsync();
			return Ok(accounts);
		}
		// GET: api/admin/accounts?search=...&role=...&page=1&pageSize=20
		[HttpGet]
		public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts(string? search = null, string? role = null, int page = 1, int pageSize = 20)
		{
			var query = from a in _context.Accounts
						where a.Role != "Admin" // Add this condition to exclude Admin accounts
						join p in _context.Parents on a.UserID equals p.UserID into parentJoin
						from parent in parentJoin.DefaultIfEmpty()
						join n in _context.Nurses on a.UserID equals n.UserID into nurseJoin
						from nurse in nurseJoin.DefaultIfEmpty()
						join m in _context.ManagerAdmins on a.UserID equals m.UserID into managerJoin
						from manager in managerJoin.DefaultIfEmpty()
						select new { a, parent, nurse, manager };

			if (!string.IsNullOrEmpty(search))
				query = query.Where(x => (x.a.Role == "Parent" && x.parent.FullName.Contains(search)) ||
										(x.a.Role == "Nurse" && x.nurse.FullName.Contains(search)));
			if (!string.IsNullOrEmpty(role))
				query = query.Where(x => x.a.Role == role);

			var total = await query.CountAsync();
			var accounts = await query
				.OrderBy(x => x.a.UserID)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.Select(x => new AccountDTOfullName
				{
					UserID = x.a.UserID,
					Username = x.a.Username,
					Role = x.a.Role,
					Active = x.a.Active,
					FullName = x.a.Role == "Parent" ? x.parent.FullName :
							  x.a.Role == "Nurse" ? x.nurse.FullName :
							  null
				})
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
				Role = request.Role,

				Active = request.Active
			};
			_context.Accounts.Add(account);
			await _context.SaveChangesAsync();
			return Ok(new AccountDTO { UserID = account.UserID, Username = account.Username, Role = account.Role, Active = account.Active });
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

			if (request.Active.HasValue)
				account.Active = request.Active.Value;
			await _context.SaveChangesAsync();
			return Ok(new AccountDTO { UserID = account.UserID, Username = account.Username, Role = account.Role, Active = account.Active });
		}

		// DELETE: api/admin/accounts/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAccount(int id)
		{
			try
			{
				var account = await _context.Accounts.FindAsync(id);
				if (account == null)
					return NotFound(new { message = "Account not found" });

				// Set Active to false instead of deleting the account
				account.Active = false;
				await _context.SaveChangesAsync();

				return Ok(new { message = "Account deactivated successfully" });
			}
			catch (Exception ex)
			{
				// Log the exception (if logging is implemented)
				return StatusCode(500, new { message = "Error occurred while deactivating account", error = ex.Message });
			}
		}
	}
}
