namespace FinControl.API.DTOs;

public record CategoryDto(
    Guid Id,
    string Nome
);

public record CreateCategoryDto(
    string Nome
);
