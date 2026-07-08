namespace FinControl.API.Models;

public class BudgetLimit
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public decimal Limite { get; set; }
}
