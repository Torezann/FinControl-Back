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
[Route("api/transactions")]
public class TransactionsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? month)
    {
        var userId = this.GetUserId();
        var query = db.Transactions.Where(t => t.UserId == userId);

        if (!string.IsNullOrWhiteSpace(month))
        {
            if (!DateOnly.TryParseExact(month + "-01", "yyyy-MM-dd", out var monthStart))
                return Problem(title: "Formato de mês inválido, use yyyy-MM", statusCode: StatusCodes.Status400BadRequest);

            var monthEnd = monthStart.AddMonths(1);
            query = query.Where(t => t.Data >= monthStart && t.Data < monthEnd);
        }

        var transactions = await query
            .Select(t => new TransactionDto(t.Id, t.Data, t.Tipo, t.Valor, t.Categoria, t.Descricao))
            .ToListAsync();

        return Ok(transactions);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        if (dto.Valor <= 0)
            return Problem(title: "Valor deve ser maior que zero", statusCode: StatusCodes.Status400BadRequest);

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = this.GetUserId(),
            Data = dto.Data,
            Tipo = dto.Tipo,
            Valor = dto.Valor,
            Categoria = dto.Categoria,
            Descricao = dto.Descricao
        };

        db.Transactions.Add(transaction);
        await db.SaveChangesAsync();

        var result = new TransactionDto(transaction.Id, transaction.Data, transaction.Tipo, transaction.Valor, transaction.Categoria, transaction.Descricao);
        return CreatedAtAction(nameof(GetAll), new { }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = this.GetUserId();
        var transaction = await db.Transactions.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (transaction is null) return NotFound();

        db.Transactions.Remove(transaction);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
