using backend.Models;
using backend.Models.Enums;
using backend.DTOs;
using BCrypt.Net;

namespace backend.Mapping;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    public static User ToEntity(this RegisterDto registerDto)
{
    return new User
    {
        Username = registerDto.Username,
        Email = registerDto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), // Hash password here
        Role = UserRole.Customer
    };
}
}