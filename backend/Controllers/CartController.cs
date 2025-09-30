using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;
using backend.Models;
using Shop.Shared.Results;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly CartService cartService;

    public CartController(CartService cartService)
    {
        this.cartService = cartService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCartItems(long userId)
    {
        var result = await this.cartService.GetCartItemsAsync(userId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpPost("{userId}/items")]
    public async Task<IActionResult> AddToCart(long userId, [FromBody] AddToCartRequestDto request)
    {
        // Log request details for debugging
        Console.WriteLine($"AddToCart: userId={userId}, productId={request.ProductId}, quantity={request.Quantity}");
        
        var result = await this.cartService.AddToCartAsync(userId, request.ProductId, request.Quantity);

        if (!result.IsSuccess)
        {
            Console.WriteLine($"AddToCart failed: {result.ErrorMessage}");
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    [HttpPut("{userId}/items/{cartItemId}")]
    public async Task<IActionResult> UpdateCartItemQuantity(long userId, long cartItemId, [FromBody] UpdateQuantityRequestDto request)
    {
        var result = await this.cartService.UpdateCartItemQuantityAsync(userId, cartItemId, request.Quantity);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpDelete("{userId}/items/{cartItemId}")]
    public async Task<IActionResult> RemoveFromCart(long userId, long cartItemId)
    {
        var result = await this.cartService.RemoveFromCartAsync(userId, cartItemId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return NoContent();
    }

    [HttpDelete("{userId}/clear")]
    public async Task<IActionResult> ClearCart(long userId)
    {
        var result = await this.cartService.ClearCartAsync(userId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return NoContent();
    }

    [HttpGet("{userId}/total")]
    public async Task<IActionResult> GetCartTotal(long userId)
    {
        var result = await this.cartService.GetCartTotalAsync(userId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(new { total = result.Data });
    }

    [HttpGet("{userId}/count")]
    public async Task<IActionResult> GetCartItemCount(long userId)
    {
        var result = await this.cartService.GetCartItemCountAsync(userId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(new { count = result.Data });
    }
}
