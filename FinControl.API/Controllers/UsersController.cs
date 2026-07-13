using FinControl.API.DTOs;
using FinControl.API.Extensions;
using FinControl.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinControl.API.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController(UserManager<ApplicationUser> userManager) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var user = await userManager.FindByIdAsync(this.GetUserId().ToString());
        if (user is null) return NotFound();

        return Ok(new UserProfileDto(user.Email!));
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateEmail(UpdateEmailDto dto)
    {
        var user = await userManager.FindByIdAsync(this.GetUserId().ToString());
        if (user is null) return NotFound();

        var existing = await userManager.FindByEmailAsync(dto.Email);
        if (existing is not null && existing.Id != user.Id)
        {
            ModelState.AddModelError(nameof(IdentityErrorDescriber.DuplicateEmail), "Já existe uma conta com este email.");
            return ValidationProblem(ModelState);
        }

        var emailResult = await userManager.SetEmailAsync(user, dto.Email);
        if (!emailResult.Succeeded)
        {
            foreach (var error in emailResult.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return ValidationProblem(ModelState);
        }

        var userNameResult = await userManager.SetUserNameAsync(user, dto.Email);
        if (!userNameResult.Succeeded)
        {
            foreach (var error in userNameResult.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return ValidationProblem(ModelState);
        }

        return Ok(new UserProfileDto(user.Email!));
    }

    [HttpPost("me/password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var user = await userManager.FindByIdAsync(this.GetUserId().ToString());
        if (user is null) return NotFound();

        var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);
            return ValidationProblem(ModelState);
        }

        return NoContent();
    }
}
