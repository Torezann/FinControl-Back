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
[Route("api/goals")]
public class GoalsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = this.GetUserId();
        var goals = await db.Goals
            .Where(g => g.UserId == userId)
            .Select(g => new GoalDto(g.Id, g.Nome, g.Alvo, g.Salvo))
            .ToListAsync();

        return Ok(goals);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGoalDto dto)
    {
        if (dto.Alvo <= 0)
            return Problem(title: "Alvo deve ser maior que zero", statusCode: StatusCodes.Status400BadRequest);

        var goal = new Goal
        {
            Id = Guid.NewGuid(),
            UserId = this.GetUserId(),
            Nome = dto.Nome,
            Alvo = dto.Alvo,
            Salvo = 0
        };

        db.Goals.Add(goal);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { }, new GoalDto(goal.Id, goal.Nome, goal.Alvo, goal.Salvo));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = this.GetUserId();
        var goal = await db.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
        if (goal is null) return NotFound();

        db.Goals.Remove(goal);
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{id}/deposit")]
    public async Task<IActionResult> Deposit(Guid id, DepositGoalDto dto)
    {
        if (dto.Amount <= 0)
            return Problem(title: "Amount deve ser maior que zero", statusCode: StatusCodes.Status400BadRequest);

        var userId = this.GetUserId();
        var goal = await db.Goals.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);
        if (goal is null) return NotFound();

        goal.Salvo += dto.Amount;
        await db.SaveChangesAsync();

        return Ok(new GoalDto(goal.Id, goal.Nome, goal.Alvo, goal.Salvo));
    }
}
