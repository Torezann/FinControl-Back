using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinControl.API.Data;
using FinControl.API.DTOs;
using FinControl.API.Extensions;
using FinControl.API.Models;

namespace FinControl.API.Controllers;

[Authorize]
[ApiController]
[Route("api/budget-limits")]
public class BudgetLimitsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = this.GetUserId();
        var limits = await db.BudgetLimits
            .Where(b => b.UserId == userId)
            .ToDictionaryAsync(b => b.Categoria, b => b.Limite);

        return Ok(limits);
    }

    [HttpPut("{categoria}")]
    public async Task<IActionResult> Upsert(string categoria, UpsertBudgetLimitDto dto)
    {
        if (dto.Limite <= 0)
            return Problem(title: "Limite deve ser maior que zero", statusCode: StatusCodes.Status400BadRequest);

        var userId = this.GetUserId();
        var limit = await db.BudgetLimits.FirstOrDefaultAsync(b => b.UserId == userId && b.Categoria == categoria);

        if (limit is null)
        {
            limit = new BudgetLimit { Id = Guid.NewGuid(), UserId = userId, Categoria = categoria, Limite = dto.Limite };
            db.BudgetLimits.Add(limit);
        }
        else
        {
            limit.Limite = dto.Limite;
        }

        await db.SaveChangesAsync();

        return Ok(new BudgetLimitDto(limit.Categoria, limit.Limite));
    }

    [HttpDelete("{categoria}")]
    public async Task<IActionResult> Delete(string categoria)
    {
        var userId = this.GetUserId();
        var limit = await db.BudgetLimits.FirstOrDefaultAsync(b => b.UserId == userId && b.Categoria == categoria);
        if (limit is null) return NotFound();

        db.BudgetLimits.Remove(limit);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
