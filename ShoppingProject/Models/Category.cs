namespace ShoppingProject.Models;

public class Category
{
    public string CategoryId { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "";
    public string? Description { get; set; } 
}
