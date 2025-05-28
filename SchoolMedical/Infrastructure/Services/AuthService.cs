using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolMedical.Core.DTOs.Auth;
using SchoolMedical.Core.Interfaces.Services;
using SchoolMedical.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolMedical.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                // Find user by username
                var account = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Username == request.Username);

                if (account == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    };
                }

                // Verify password
                if (!VerifyPassword(request.Password, account.PasswordHash))
                {
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
            var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]);

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
            try
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];

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
        }

        private bool VerifyPassword(string password, string hashedPassword) =>
            // For demo purposes, using simple comparison
            // In production, use proper password hashing like BCrypt
            BCrypt.Net.BCrypt.Verify(password, hashedPassword);

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
    }
}