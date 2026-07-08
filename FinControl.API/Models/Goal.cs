namespace FinControl.API.Models;

public class Goal
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Alvo { get; set; }
    public decimal Salvo { get; set; }
}
