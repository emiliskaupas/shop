namespace backend.DTOs;

public class CartItemDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public ProductDto? Product { get; set; }
    public int Quantity { get; set; }
}

public class AddToCartRequestDto
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateQuantityRequestDto
{
    public int Quantity { get; set; }
}