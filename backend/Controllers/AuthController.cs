using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService authService;

    public AuthController(AuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await authService.LoginAsync(loginDto);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var result = await authService.RegisterAsync(registerDto);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.ErrorMessage });

        return Ok(result.Data);
    }
}