using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Models.Enums;
using Shop.Shared.Results;
using Shop.Shared.Validation;
using Shop.Shared.Notifications;
using Microsoft.EntityFrameworkCore;
using backend.Mapping;

namespace backend.Services;

public class AuthService : BaseService
{
    private readonly INotificationService notificationService;

    public AuthService(AppDbContext shopContext, INotificationService notificationService) : base(shopContext)
    {
        this.notificationService = notificationService;
    }

    public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            // Validate input using your existing validation extensions
            var emailValidation = loginDto.Email.ValidateNotEmpty("Email");
            if (!emailValidation.IsSuccess)
                return Result<UserDto>.Failure(emailValidation.ErrorMessage!);

            var passwordValidation = loginDto.Password.ValidateNotEmpty("Password");
            if (!passwordValidation.IsSuccess)
                return Result<UserDto>.Failure(passwordValidation.ErrorMessage!);

            // Find user by email
            var user = await shopContext.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
                return Result<UserDto>.Failure("Invalid email or password");

            // In production, use proper password hashing (BCrypt, etc.)
            if (user.PasswordHash != loginDto.Password)
                return Result<UserDto>.Failure("Invalid email or password");

            // Map to DTO using existing pattern
            var userDto = user.ToDto();

            // Trigger login notification
            await notificationService.SendNotificationAsync(
                $"User {user.Username} logged in successfully", 
                NotificationType.Success);

            return Result<UserDto>.Success(userDto);
        }
        catch
        {
            return Result<UserDto>.Failure("Authentication failed");
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
}