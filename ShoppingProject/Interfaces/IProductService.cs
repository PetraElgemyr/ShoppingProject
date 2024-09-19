using ShoppingApp.Models;

namespace ShoppingApp.Interfaces;

internal interface IProductService
{
    public Response GetAllProducts();
    public Response AddNewProductToList(Product productRequest);
    public Response UpdateProductById(Product updatedProduct);
    public Response DeleteProductById(string id);
    public Response GetAllProductsByCategoryId(string categoryId);

    public Response DeleteProductsByCategoryId(string categoryId);
    
}
