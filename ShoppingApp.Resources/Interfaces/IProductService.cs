using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Interfaces;

public interface IProductService
{
    RequestResponse<Product> CreateAndAddProductToList(Product productRequest);
    RequestResponse<Product> DeleteProductById(string id);
    RequestResponse<Product> DeleteProductsWithSpecificCategoryId(string categoryId);
    RequestResponse<IEnumerable<Product>> GetAllProducts();
    RequestResponse<Product> GetOneProductById(string id);
    RequestResponse<Product> UpdateProductById(string id, Product updatedProduct);
}
