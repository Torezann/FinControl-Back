using FinControl.API.Models;

namespace FinControl.API.DTOs;

public record TransactionDto(
    Guid Id,
    DateOnly Data,
    TransactionType Tipo,
    decimal Valor,
    string Categoria,
    string Descricao
);

public record CreateTransactionDto(
    TransactionType Tipo,
    decimal Valor,
    string Categoria,
    string Descricao,
    DateOnly Data
);
