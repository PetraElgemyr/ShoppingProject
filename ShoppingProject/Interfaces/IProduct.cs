namespace ShoppingApp.Interfaces;

public interface IProduct
{
    public string Id { get; set; }
    public string Name { get; set; } 
    public int Price { get; set; }
    public string Description { get; set; }

    public string CategoryId { get; set; }
}
