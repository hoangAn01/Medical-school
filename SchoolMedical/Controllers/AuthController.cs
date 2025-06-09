using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolMedical.Core.DTOs.Auth;
using SchoolMedical.Core.Interfaces.Services;
using SchoolMedical.Infrastructure.Data;
using SchoolMedical.Infrastructure.Services;

namespace SchoolMedical.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase{
		private readonly IAuthService _authService;
		private readonly ILogger<AuthController> _logger;

		public AuthController(IAuthService authService, ILogger<AuthController> logger)
		{
			_authService = authService;
			_logger = logger;
		}

		/// Test database connection and check accounts
		[HttpGet("test-db")]
		public async Task<ActionResult> TestDatabase()
		{
			try
			{
				_logger.LogInformation("Testing database connection...");

				// Test qua AuthService thay vì direct context
				var testResult = await _authService.TestDatabaseConnectionAsync();

				return Ok(testResult);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Database test failed");
				return BadRequest(new { error = ex.Message, stackTrace = ex.StackTrace });
			}
		}

		/// User login endpoint
		[HttpPost("login")]
		public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request){
			try{
				_logger.LogInformation($"Login request received for username: {request.Username}");

				if (!ModelState.IsValid){
					_logger.LogWarning("Invalid model state for login request");
					return BadRequest(new LoginResponse{
						Success = false,
						Message = "Invalid input data"
					});
				}

				var result = await _authService.LoginAsync(request);

				if (!result.Success)
				{
					_logger.LogWarning($"Login failed for username: {request.Username}. Reason: {result.Message}");
					return Unauthorized(result);
				}

				_logger.LogInformation($"Login successful for username: {request.Username}");
				return Ok(result);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Exception in login endpoint for username: {request.Username}");
				return StatusCode(500, new LoginResponse
				{
					Success = false,
					Message = $"Server error: {ex.Message}",
					Token = null,
					User = null
				});
			}
		}

		/// Validate JWT token
		[HttpPost("validate-token")]
		public async Task<ActionResult<bool>> ValidateToken([FromBody] string token)
		{
			var isValid = await _authService.ValidateTokenAsync(token);
			return Ok(isValid);
		}

		/// Get current user info (requires authentication)
		[HttpGet("me")]
		[Authorize]
		public ActionResult<object> GetCurrentUser()
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
			var username = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
			var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

			return Ok(new
			{
				UserID = userId,
				Username = username,
				Role = role
			});
		}

		/// Logout endpoint (client-side token removal)
		[HttpPost("logout")]
		[Authorize]
		public ActionResult Logout(){
			return Ok(new { message = "Logout successful" });
		}


		[HttpPost("register")]
		public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new LoginResponse
				{
					Success = false,
					Message = "Invalid input data"
				});
			}

			var result = await _authService.RegisterAsync(request);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result);
		}
	}
}
