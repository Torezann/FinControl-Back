namespace FinControl.API.DTOs;

public record GoalDto(
    Guid Id,
    string Nome,
    decimal Alvo,
    decimal Salvo
);

public record CreateGoalDto(
    string Nome,
    decimal Alvo
);

public record DepositGoalDto(
    decimal Amount
);
