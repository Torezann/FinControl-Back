namespace FinControl.API.DTOs;

public record BudgetLimitDto(
    string Categoria,
    decimal Limite
);

public record UpsertBudgetLimitDto(
    decimal Limite
);
