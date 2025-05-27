using DemoBackend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase{
		private readonly DataContext _context;

		public LoginController(DataContext context){
			_context = context;
		}

		// GET: api/login
		[HttpGet]
		public IActionResult GetAllAccounts(){
			var accounts = _context.Users.ToList();

			foreach (var account in accounts){
				Console.WriteLine($"👤 UserID: {account.UserID}, Username: {account.Username}, Role: {account.Role}");
			}

			return Ok(accounts); // Trả dữ liệu về frontend nếu cần
		}
	}
}
