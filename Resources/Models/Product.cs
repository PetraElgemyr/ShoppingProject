using Resources.Interfaces;

namespace Resources.Models;

public class Product : IProduct
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public int Price { get; set; }
    public string? Description { get; set; }
    public string? CategoryId { get; set; }
}
