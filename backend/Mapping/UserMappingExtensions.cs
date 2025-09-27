using backend.Models;
using backend.Models.Enums;
using backend.DTOs;

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
            PasswordHash = registerDto.Password, // Convert: DTO.Password → Model.PasswordHash
            //PasswordHash = BCrypt.HashPassword(registerDto.Password) // Hashing can be done here if needed
            Role = UserRole.Customer
        };
    }
}