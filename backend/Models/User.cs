namespace backend.Models;
using backend.Models.Enums;
public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;  // Changed from Password to PasswordHash
    public UserRole Role { get; set; } = UserRole.Customer;
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}