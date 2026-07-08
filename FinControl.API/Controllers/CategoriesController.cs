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
[Route("api/categories")]
public class CategoriesController(AppDbContext db) : ControllerBase
{
    private static readonly string[] DefaultCategories =
    [
        "Alimentação",
        "Moradia",
        "Transporte",
        "Lazer",
        "Saúde",
        "Compras",
        "Outros",
    ];

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = this.GetUserId();
        var categories = await db.Categories
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Nome)
            .ToListAsync();

        if (categories.Count == 0)
        {
            categories = DefaultCategories
                .Select(nome => new Category { Id = Guid.NewGuid(), UserId = userId, Nome = nome })
                .ToList();
            db.Categories.AddRange(categories);
            await db.SaveChangesAsync();
        }

        return Ok(categories.Select(c => new CategoryDto(c.Id, c.Nome)));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var nome = dto.Nome.Trim();
        if (string.IsNullOrEmpty(nome))
            return Problem(title: "Nome da categoria é obrigatório", statusCode: StatusCodes.Status400BadRequest);

        var userId = this.GetUserId();
        var exists = await db.Categories.AnyAsync(c => c.UserId == userId && c.Nome == nome);
        if (exists)
            return Problem(title: "Categoria já existe", statusCode: StatusCodes.Status409Conflict);

        var category = new Category { Id = Guid.NewGuid(), UserId = userId, Nome = nome };
        db.Categories.Add(category);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { }, new CategoryDto(category.Id, category.Nome));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = this.GetUserId();
        var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
        if (category is null) return NotFound();

        var limit = await db.BudgetLimits.FirstOrDefaultAsync(b => b.UserId == userId && b.Categoria == category.Nome);
        if (limit is not null) db.BudgetLimits.Remove(limit);

        db.Categories.Remove(category);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
