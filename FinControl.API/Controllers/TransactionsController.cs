using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinControl.API.Data;
using FinControl.API.DTOs;
using FinControl.API.Models;

namespace FinControl.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await db.Transactions.ToListAsync();
        return Ok(transactions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transaction = await db.Transactions.FindAsync(id);
        if (transaction is null) return NotFound();
        return Ok(transaction);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTransactionDto dto)
    {
        var transaction = new Transaction
        {
            Description = dto.Description,
            Amount = dto.Amount,
            Date = dto.Date,
            Type = dto.Type
        };

        db.Transactions.Add(transaction);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateTransactionDto dto)
    {
        var transaction = await db.Transactions.FindAsync(id);
        if (transaction is null) return NotFound();

        transaction.Description = dto.Description;
        transaction.Amount = dto.Amount;
        transaction.Date = dto.Date;
        transaction.Type = dto.Type;

        await db.SaveChangesAsync();
        return Ok(transaction);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var transaction = await db.Transactions.FindAsync(id);
        if (transaction is null) return NotFound();

        db.Transactions.Remove(transaction);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
