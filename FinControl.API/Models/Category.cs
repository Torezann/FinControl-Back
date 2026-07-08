namespace FinControl.API.Models;

public class Category
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Nome { get; set; } = string.Empty;
}
