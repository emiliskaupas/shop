using Microsoft.AspNetCore.Mvc;
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

    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] CreateProductDto product)
    {
        var result = await this.productService.AddProductAsync(product);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return CreatedAtAction(nameof(GetProductById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(long id, [FromBody] UpdateProductDto product)
    {
        var result = await this.productService.UpdateProductAsync(id, product);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(long id)
    {
        var result = await this.productService.DeleteProductAsync(id);

        if (!result.IsSuccess)
            return NotFound(new { error = result.ErrorMessage });

        return NoContent();
    }
}
