using SchoolMedical.Core.DTOs.Auth;

namespace SchoolMedical.Core.Interfaces.Services
{
	public interface IAuthService
	{
		Task<LoginResponse> LoginAsync(LoginRequest request);
		Task<bool> ValidateTokenAsync(string token);
		string GenerateJwtToken(int userId, string username, string role);
		Task<object> TestDatabaseConnectionAsync();

		// Register
		Task<LoginResponse> RegisterAsync(RegisterRequest request);
	}
}
