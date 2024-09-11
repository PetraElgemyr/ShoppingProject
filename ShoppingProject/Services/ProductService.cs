using ShoppingProject.Models;
using System.Diagnostics;

namespace ShoppingProject.Services;

internal class ProductService
{
    private List<Product> products = [];
    internal void AddOrUpdateProduct(Product productRequest)
    {

    }

    internal IEnumerable<Product> GetAllProducts()
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

    internal void DeleteProductById(string id)
    {

    }

    internal void DeleteProductsByCategoryId(string categoryId)
    {

    }
}
