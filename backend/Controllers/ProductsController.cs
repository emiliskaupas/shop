using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.DTOs;
using backend.Services;
using Shop.Shared.Pagination;
using Shop.Shared.Results;
namespace backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService productService;

    public ProductsController(ProductService productService)
    {
        this.productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] PaginationRequest request)
    {
        var result = await this.productService.GetProductsAsync(request);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(long id)
    {
        var result = await this.productService.GetProductByIdAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpGet("my-products")]
    [Authorize] // Require authentication to see own products
    public async Task<IActionResult> GetMyProducts([FromQuery] PaginationRequest request)
    {
        // Get current user ID from JWT claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return Unauthorized(new { error = "Invalid user authentication" });
        }

        var result = await this.productService.GetProductsByUserAsync(userId, request);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize] // Require authentication for creating products
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto product)
    {
        // Get current user ID from JWT claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return Unauthorized(new { error = "Invalid user authentication" });
        }

        var result = await this.productService.AddProductAsync(product, userId);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return CreatedAtAction(nameof(GetProductById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    [Authorize] // Require authentication for updating products
    public async Task<IActionResult> UpdateProduct(long id, [FromBody] UpdateProductDto product)
    {
        // Get current user ID from JWT claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return Unauthorized(new { error = "Invalid user authentication" });
        }

        var result = await this.productService.UpdateProductAsync(id, product, userId);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage == "Product not found")
                return NotFound(new { error = result.ErrorMessage });
            if (result.ErrorMessage == "You can only modify your own products")
                return Forbid(result.ErrorMessage);
            return BadRequest(new { error = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    [Authorize] // Require authentication for deleting products
    public async Task<IActionResult> DeleteProduct(long id)
    {
        // Get current user ID from JWT claims
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out long userId))
        {
            return Unauthorized(new { error = "Invalid user authentication" });
        }

        var result = await this.productService.DeleteProductAsync(id, userId);

        if (!result.IsSuccess)
        {
            if (result.ErrorMessage == "Product not found")
                return NotFound(new { error = result.ErrorMessage });
            if (result.ErrorMessage == "You can only delete your own products")
                return Forbid(result.ErrorMessage);
            return BadRequest(new { error = result.ErrorMessage });
        }

        return NoContent();
    }
}
