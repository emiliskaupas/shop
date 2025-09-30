namespace backend.Services;

using backend.Data;
using backend.Models;
using backend.DTOs;
using backend.Mapping;
using Microsoft.EntityFrameworkCore;
using Shop.Shared.Results;
using Shop.Shared.Validation;

public class CartService : BaseService
{
    public CartService(AppDbContext shopContext) : base(shopContext)
    { }

    public async Task<Result<List<CartItemDto>>> GetCartItemsAsync(long userId)
    {
        try
        {
            var cartItems = await shopContext.CartItems
                .Include(ci => ci.Product!)
                    .ThenInclude(p => p.CreatedBy)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            var cartItemDtos = cartItems.Select(ci => ci.ToDto()).ToList();
            return Result<List<CartItemDto>>.Success(cartItemDtos);
        }
        catch (Exception ex)
        {
            return Result<List<CartItemDto>>.Failure(ex);
        }
    }

    public async Task<Result<CartItemDto>> AddToCartAsync(long userId, long productId, int quantity)
    {
        try
        {
            Console.WriteLine($"CartService.AddToCartAsync: userId={userId}, productId={productId}, quantity={quantity}");
            
            // Validate inputs
            var quantityValidation = quantity.ValidateRange(1, 100, "Quantity");
            if (!quantityValidation.IsSuccess)
            {
                Console.WriteLine($"Quantity validation failed: {quantityValidation.ErrorMessage}");
                return Result<CartItemDto>.Failure(quantityValidation.ErrorMessage!);
            }

            // Check if product exists
            var product = await shopContext.Products.FindAsync(productId);
            if (product == null)
            {
                Console.WriteLine($"Product not found: productId={productId}");
                return Result<CartItemDto>.Failure("Product not found");
            }

            // Check if user exists
            var user = await shopContext.Users.FindAsync(userId);
            if (user == null)
            {
                Console.WriteLine($"User not found: userId={userId}");
                return Result<CartItemDto>.Failure("User not found");
            }

            Console.WriteLine($"Validation passed. Creating/updating cart item...");
            
            // Check if item already exists in cart
            var existingCartItem = await shopContext.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

            if (existingCartItem != null)
            {
                // Update quantity
                existingCartItem.Quantity += quantity;
                await shopContext.SaveChangesAsync();
                
                // Reload with product and user information
                await shopContext.Entry(existingCartItem)
                    .Reference(ci => ci.Product)
                    .LoadAsync();
                    
                if (existingCartItem.Product != null)
                {
                    await shopContext.Entry(existingCartItem.Product)
                        .Reference(p => p.CreatedBy)
                        .LoadAsync();
                }
                
                return Result<CartItemDto>.Success(existingCartItem.ToDto());
            }
            else
            {
                // Create new cart item
                var newCartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };

                await shopContext.CartItems.AddAsync(newCartItem);
                await shopContext.SaveChangesAsync();
                
                // Reload with product and user information
                await shopContext.Entry(newCartItem)
                    .Reference(ci => ci.Product)
                    .LoadAsync();
                    
                if (newCartItem.Product != null)
                {
                    await shopContext.Entry(newCartItem.Product)
                        .Reference(p => p.CreatedBy)
                        .LoadAsync();
                }

                return Result<CartItemDto>.Success(newCartItem.ToDto());
            }
        }
        catch (Exception ex)
        {
            return Result<CartItemDto>.Failure(ex);
        }
    }

    public async Task<Result<CartItemDto>> UpdateCartItemQuantityAsync(long userId, long cartItemId, int quantity)
    {
        try
        {
            // Validate quantity
            var quantityValidation = quantity.ValidateRange(1, 100, "Quantity");
            if (!quantityValidation.IsSuccess)
                return Result<CartItemDto>.Failure(quantityValidation.ErrorMessage!);

            var cartItem = await shopContext.CartItems
                .Include(ci => ci.Product!)
                    .ThenInclude(p => p.CreatedBy)
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem == null)
                return Result<CartItemDto>.Failure("Cart item not found");

            // Check if user is trying to update quantity of their own product
            if (cartItem.Product != null && cartItem.Product.CreatedByUserId == userId)
                return Result<CartItemDto>.Failure("You cannot modify cart items containing your own products");

            cartItem.Quantity = quantity;
            await shopContext.SaveChangesAsync();

            return Result<CartItemDto>.Success(cartItem.ToDto());
        }
        catch (Exception ex)
        {
            return Result<CartItemDto>.Failure(ex);
        }
    }

    public async Task<Result> RemoveFromCartAsync(long userId, long cartItemId)
    {
        try
        {
            var cartItem = await shopContext.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.UserId == userId);

            if (cartItem == null)
                return Result.Failure("Cart item not found");

            shopContext.CartItems.Remove(cartItem);
            await shopContext.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    public async Task<Result> ClearCartAsync(long userId)
    {
        try
        {
            var cartItems = await shopContext.CartItems
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (cartItems.Any())
            {
                shopContext.CartItems.RemoveRange(cartItems);
                await shopContext.SaveChangesAsync();
            }

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    public async Task<Result<decimal>> GetCartTotalAsync(long userId)
    {
        try
        {
            var cartItems = await shopContext.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            var total = cartItems.Sum(ci => ci.Product!.Price * ci.Quantity);

            return Result<decimal>.Success(total);
        }
        catch (Exception ex)
        {
            return Result<decimal>.Failure(ex);
        }
    }

    public async Task<Result<int>> GetCartItemCountAsync(long userId)
    {
        try
        {
            var count = await shopContext.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);

            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(ex);
        }
    }
}