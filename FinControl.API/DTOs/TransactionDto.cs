using FinControl.API.Models;

namespace FinControl.API.DTOs;

public record CreateTransactionDto(
    string Description,
    decimal Amount,
    DateTime Date,
    TransactionType Type
);
