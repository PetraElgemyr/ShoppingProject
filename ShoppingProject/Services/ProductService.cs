using ShoppingApp.Models;
using System.Diagnostics;

namespace ShoppingApp.Services;

public class ProductService
{
    private List<Product> products = [];
    public void AddNewProduct(Product productRequest)
    {
        try
        {

        } catch ( Exception ex)
        {

        }
    }

    public void UpdateProduct()
    {

    }

    public IEnumerable<Product> GetAllProducts()
    {
        try
        {
            return products;
        }
        catch (Exception ex)
        {
            Debug.Write(ex.Message);
        }
        return null!;
    }

    public void DeleteProductById(string id)
    {

    }

    public void DeleteProductsByCategoryId(string categoryId)
    {

    }
}
