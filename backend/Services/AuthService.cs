using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Models.Enums;
using Shop.Shared.Results;
using Shop.Shared.Validation;
using Shop.Shared.Notifications;
using Microsoft.EntityFrameworkCore;
using backend.Mapping;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;

namespace backend.Services;

public class AuthService : BaseService
{
    private readonly INotificationService notificationService;
    private readonly IConfiguration configuration;

    public AuthService(AppDbContext shopContext, INotificationService notificationService, IConfiguration configuration) : base(shopContext)
    {
        this.notificationService = notificationService;
        this.configuration = configuration;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            // Validate input using your existing validation extensions
            var emailValidation = loginDto.Email.ValidateNotEmpty("Email");
            if (!emailValidation.IsSuccess)
                return Result<LoginResponseDto>.Failure(emailValidation.ErrorMessage!);

            var passwordValidation = loginDto.Password.ValidateNotEmpty("Password");
            if (!passwordValidation.IsSuccess)
                return Result<LoginResponseDto>.Failure(passwordValidation.ErrorMessage!);

            // Find user by email
            var user = await shopContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                return Result<LoginResponseDto>.Failure("Invalid email or password");

            // In production, use proper password hashing (BCrypt, etc.)
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return Result<LoginResponseDto>.Failure("Invalid email or password");

            // Generate JWT token
            var token = GenerateJwtToken(user);
            var expiresAt = DateTime.UtcNow.AddHours(24); // 24 hour expiration

            // Map to DTO using existing pattern
            var userDto = user.ToDto();
            var loginResponse = new LoginResponseDto
            {
                User = userDto,
                Token = token,
                ExpiresAt = expiresAt
            };

            // Trigger login notification
            await notificationService.SendNotificationAsync(
                $"User {user.Username} logged in successfully", 
                NotificationType.Success);

            return Result<LoginResponseDto>.Success(loginResponse);
        }
        catch
        {
            return Result<LoginResponseDto>.Failure("Authentication failed");
        }
    }

    public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            Console.WriteLine($"Starting registration for: {registerDto.Username}, {registerDto.Email}");
            
            // Validate input using existing validation extensions
            var usernameValidation = registerDto.Username.ValidateNotEmpty("Username");
            if (!usernameValidation.IsSuccess)
                return Result<UserDto>.Failure(usernameValidation.ErrorMessage!);

            var emailValidation = registerDto.Email.ValidateNotEmpty("Email");
            if (!emailValidation.IsSuccess)
                return Result<UserDto>.Failure(emailValidation.ErrorMessage!);

            var passwordValidation = registerDto.Password.ValidateNotEmpty("Password");
            if (!passwordValidation.IsSuccess)
                return Result<UserDto>.Failure(passwordValidation.ErrorMessage!);

            Console.WriteLine("Validation passed, checking for existing users...");

            // Check if user already exists by email
            var existingUserByEmail = await shopContext.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (existingUserByEmail != null)
                return Result<UserDto>.Failure("User with this email already exists");

            // Check if user already exists by username
            var existingUserByUsername = await shopContext.Users
                .FirstOrDefaultAsync(u => u.Username == registerDto.Username);

            if (existingUserByUsername != null)
                return Result<UserDto>.Failure("Username is already taken");

            Console.WriteLine("No existing users found, creating new user...");

            // Create new user
            var user = registerDto.ToEntity();
            Console.WriteLine($"User entity created: {user.Username}, {user.Email}");

            shopContext.Users.Add(user);
            Console.WriteLine("User added to context, saving changes...");
            
            await shopContext.SaveChangesAsync();
            Console.WriteLine("Changes saved successfully");

            // Map to DTO using existing pattern
            var userDto = user.ToDto();
            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            // Log the actual exception for debugging
            Console.WriteLine($"Registration error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return Result<UserDto>.Failure($"Registration failed: {ex.Message}");
        }
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = configuration["JwtSettings:SecretKey"] ?? "your-very-long-secret-key-that-should-be-at-least-256-bits-long-for-security-purposes";
        var key = Encoding.ASCII.GetBytes(jwtKey);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}