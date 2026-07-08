namespace FinControl.API.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateOnly Data { get; set; }
    public TransactionType Tipo { get; set; }
    public decimal Valor { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}

public enum TransactionType
{
    Receita,
    Gasto
}
