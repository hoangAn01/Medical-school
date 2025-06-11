using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolMedical.Core.DTOs.Auth;
using SchoolMedical.Core.Entities;
using SchoolMedical.Core.Interfaces.Services;
using SchoolMedical.Infrastructure.Data;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolMedical.Infrastructure.Services
{
	public class AuthService : IAuthService
	{
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;
		private readonly ILogger<AuthService> _logger;

		public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
		{
			_context = context;
			_configuration = configuration;
			_logger = logger;
		}

		public async Task<LoginResponse> LoginAsync(LoginRequest request){
			try{
				_logger.LogInformation($"Login attempt for username: {request.Username}");

				// Find user by username
				var account = await _context.Accounts
					.FirstOrDefaultAsync(a => a.Username == request.Username);

				if (account == null){
					_logger.LogWarning($"User not found: {request.Username}");
					return new LoginResponse
					{
						Success = false,
						Message = "Invalid username or password"
					};	
				}

				_logger.LogInformation($"User found: {account.Username}, Role: {account.Role}");
				_logger.LogInformation($"Stored hash: {account.PasswordHash}");

				// Verify password
				bool passwordValid = VerifyPassword(request.Password, account.PasswordHash);
				_logger.LogInformation($"Password verification result: {passwordValid}");

				if (!passwordValid)
				{
					_logger.LogWarning($"Invalid password for user: {request.Username}");
					return new LoginResponse
					{
						Success = false,
						Message = "Invalid username or password"
					};
				}

				// Get user details based on role
				var userInfo = await GetUserInfoAsync(account);

				// Generate JWT token
				var token = GenerateJwtToken(account.UserID, account.Username, account.Role);

				_logger.LogInformation($"Login successful for user: {request.Username}");

				return new LoginResponse
				{
					Success = true,
					Message = "Login successful",
					Token = token,
					User = userInfo
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Error during login for user: {request.Username}");
				return new LoginResponse
				{
					Success = false,
					Message = "An error occurred during login"
				};
			}
		}

		public string GenerateJwtToken(int userId, string username, string role)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings");
			var secretKey = jwtSettings["SecretKey"];
			var issuer = jwtSettings["Issuer"];
			var audience = jwtSettings["Audience"];
			var expiryMinutesValue = jwtSettings["ExpiryMinutes"];

			if (string.IsNullOrWhiteSpace(secretKey))
				throw new InvalidOperationException("JWT SecretKey is not configured.");
			if (string.IsNullOrWhiteSpace(issuer))
				throw new InvalidOperationException("JWT Issuer is not configured.");
			if (string.IsNullOrWhiteSpace(audience))
				throw new InvalidOperationException("JWT Audience is not configured.");
			if (string.IsNullOrWhiteSpace(expiryMinutesValue) || !int.TryParse(expiryMinutesValue, out var expiryMinutes))
				throw new InvalidOperationException("JWT ExpiryMinutes is not configured or invalid.");

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
				new Claim(ClaimTypes.Name, username),
				new Claim(ClaimTypes.Role, role),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
			};

			var token = new JwtSecurityToken(
				issuer: issuer,
				audience: audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<bool> ValidateTokenAsync(string token)
		{
			return await Task.Run(() =>
			{
				try
				{
					var jwtSettings = _configuration.GetSection("JwtSettings");
					var secretKey = jwtSettings["SecretKey"];

					if (string.IsNullOrWhiteSpace(secretKey))
						return false;

					var tokenHandler = new JwtSecurityTokenHandler();
					var key = Encoding.UTF8.GetBytes(secretKey);

					tokenHandler.ValidateToken(token, new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuer = true,
						ValidIssuer = jwtSettings["Issuer"],
						ValidateAudience = true,
						ValidAudience = jwtSettings["Audience"],
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero
					}, out SecurityToken validatedToken);

					return true;
				}
				catch
				{
					return false;
				}
			});
		}

		private bool VerifyPassword(string password, string hashedPassword)
		{
			try
			{
				_logger.LogInformation($"Verifying password. Input: {password}");
				_logger.LogInformation($"Against hash: {hashedPassword}");

				// Kiểm tra nếu là hash BCrypt thật
				if (hashedPassword.StartsWith("$2a$") || hashedPassword.StartsWith("$2b$") || hashedPassword.StartsWith("$2y$"))
				{
					bool result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
					_logger.LogInformation($"BCrypt verification result: {result}");
					return result;
				}

				// Fallback cho dữ liệu mẫu (tạm thời)
				var tempPasswords = new Dictionary<string, string>
				{
					{ "hashed_password_1", "Admin123!" },
					{ "hashed_password_2", "Nurse123!" },
					{ "hashed_password_3", "Parent123!" },
					{ "hashed_password_4", "Admin123!" },
					{ "hashed_password_5", "Nurse123!" },
					{ "hashed_password_6", "Parent123!" },
					{ "hashed_password_7", "Parent123!" },
					{ "hashed_password_8", "Nurse123!" }
				};

				if (tempPasswords.ContainsKey(hashedPassword))
				{
					bool result = tempPasswords[hashedPassword] == password;
					_logger.LogInformation($"Temp password verification result: {result}");
					return result;
				}

				_logger.LogWarning($"Unknown password format: {hashedPassword}");
				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error verifying password");
				return false;
			}
		}

		private async Task<UserInfo> GetUserInfoAsync(Core.Entities.Account account)
		{
			var userInfo = new UserInfo
			{
				UserID = account.UserID,
				Username = account.Username,
				Role = account.Role
			};

			// Get full name based on role
			switch (account.Role.ToLower())
			{
				case "admin":
					var admin = await _context.ManagerAdmins
						.FirstOrDefaultAsync(m => m.UserID == account.UserID);
					userInfo.FullName = admin?.FullName;
					break;

				case "nurse":
					var nurse = await _context.Nurses
						.FirstOrDefaultAsync(n => n.UserID == account.UserID);
					userInfo.FullName = nurse?.FullName;
					break;

				case "parent":
					var parent = await _context.Parents
						.FirstOrDefaultAsync(p => p.UserID == account.UserID);
					userInfo.FullName = parent?.FullName;
					break;
			}

			return userInfo;
		}

		public async Task<object> TestDatabaseConnectionAsync()
		{
			try
			{
				_logger.LogInformation("Testing database connection...");

				// Test connection
				var canConnect = await _context.Database.CanConnectAsync();
				if (!canConnect)
				{
					return new { error = "Cannot connect to database" };
				}

				// Get all accounts
				var accounts = await _context.Accounts.ToListAsync();

				return new
				{
					message = "Database connection successful",
					accountCount = accounts.Count,
					accounts = accounts.Select(a => new
					{
						a.UserID,
						a.Username,
						a.Role,
						PasswordHashLength = a.PasswordHash?.Length,
						PasswordHashPreview = a.PasswordHash?.Substring(0, Math.Min(20, a.PasswordHash.Length)) + "..."
					})
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Database test failed");
				throw;
			}
		}

		public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
		{
			// Check if username already exists
			if (await _context.Accounts.AnyAsync(a => a.Username == request.Username))
			{
				return new LoginResponse
				{
					Success = false,
					Message = "Username already exists"
				};
			}

			// Hash the password
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

			// Always set role to "parent"
			var account = new Account
			{
				Username = request.Username,
				PasswordHash = hashedPassword,
				Role = "parent"
			};

			_context.Accounts.Add(account);
			await _context.SaveChangesAsync();

			// Create Parent entity only
			_context.Parents.Add(new Parent
			{
				FullName = request.FullName,
				Gender = request.Gender,
				DateOfBirth = request.DateOfBirth,
				Address = request.Address,
				Phone = request.Phone,
				UserID = account.UserID // This links Parent to Account
			});

			await _context.SaveChangesAsync();

			// Optionally, auto-login after registration
			var token = GenerateJwtToken(account.UserID, account.Username, account.Role);

			return new LoginResponse
			{
				Success = true,
				Message = "Registration successful",
				Token = token,
				User = new UserInfo
				{
					UserID = account.UserID,
					Username = account.Username,
					Role = account.Role,
					FullName = request.FullName
				}
			};
		}
	}
}