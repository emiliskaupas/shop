namespace backend.Models;

public class CartItem : BaseEntity
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
}