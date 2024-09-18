namespace ShoppingApp.Models;

public class Category
{
    public string CategoryId { get; private set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public string? Description { get; set; } 
}
