using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Infrastructure.Data;

namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SimpleTestController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<SimpleTestController> _logger;

		public SimpleTestController(ApplicationDbContext context, ILogger<SimpleTestController> logger)
		{
			_context = context;
			_logger = logger;
		}

		[HttpGet("ping")]
		public ActionResult Ping()
		{
			return Ok(new { message = "API is running", timestamp = DateTime.Now });
		}

		[HttpGet("db-simple")]
		public async Task<ActionResult> TestDatabaseSimple()
		{
			try
			{
				var canConnect = await _context.Database.CanConnectAsync();
				return Ok(new { canConnect, message = canConnect ? "DB Connected" : "DB Not Connected" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[HttpGet("accounts-count")]
		public async Task<ActionResult> GetAccountsCount()
		{
			try
			{
				var count = await _context.Accounts.CountAsync();
				return Ok(new { accountCount = count });
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}

		[HttpGet("accounts-list")]
		public async Task<ActionResult> GetAccountsList()
		{
			try
			{
				var accounts = await _context.Accounts
					.Select(a => new { a.UserID, a.Username, a.Role })
					.ToListAsync();
				return Ok(accounts);
			}
			catch (Exception ex)
			{
				return BadRequest(new { error = ex.Message });
			}
		}
	}
}
