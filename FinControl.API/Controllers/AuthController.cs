using FinControl.API.DTOs;
using FinControl.API.Models;
using FinControl.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinControl.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    JwtTokenService tokenService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
        var result = await userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return ValidationProblem(ModelState);
        }

        var (token, expiresAt) = tokenService.GenerateToken(user);
        return Ok(new AuthResponseDto(token, expiresAt, user.Email!));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, dto.Password))
            return Problem(title: "Credenciais inválidas", statusCode: StatusCodes.Status401Unauthorized);

        var (token, expiresAt) = tokenService.GenerateToken(user);
        return Ok(new AuthResponseDto(token, expiresAt, user.Email!));
    }
}
