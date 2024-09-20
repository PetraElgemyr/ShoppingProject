

using Newtonsoft.Json;
using Resources.Interfaces;
using Resources.Models;

namespace Resources.Services;

public class ProductService : IProductService<Product, IProduct>
{
    private readonly FileService _fileService;
    private List<Product> _products;

    public ProductService(string filePath)
    {
        _fileService = new FileService(filePath);
        _products = [];
        GetAllProducts();
    }

    public Response<IProduct> CreateAndAddProductToList(Product productRequest)
    {

        try
        {
            //Glöm inte alla andra checkar innan man addar till listan och sparar till fil. 
            _products.Add(productRequest);

            var productsAsJson = JsonConvert.SerializeObject(_products);
            var response = _fileService.SaveToFile(productsAsJson);

            if (response.Succeeded)
            {
                return new Response<IProduct> { Succeeded = true, Message = "Product was successsfully created :)" };
            }
            else
            {
                return new Response<IProduct> { Succeeded = false, Message = response.Message };
            }
        }
        catch (Exception ex)
        {
            return new Response<IProduct> { Succeeded = false, Message = ex.Message };
        }
    }
    public Response<IEnumerable<IProduct>> GetAllProducts()
    {
        try
        {
            var result = _fileService.GetFromFile();

            if (result.Succeeded)
            {
                _products = JsonConvert.DeserializeObject<List<Product>>(result.Content!)!;
                return new Response<IEnumerable<IProduct>> { Succeeded = true, Content = _products };
            }
            else
            {
                return new Response<IEnumerable<IProduct>> { Succeeded = false, Message = result.Message };
            }
        }
        catch (Exception ex)
        {
            return new Response<IEnumerable<IProduct>> { Succeeded = false, Message = ex.Message };
        }
    }
    public Response<IProduct> GetOneProductById(string id)
    {
        try
        {
            var product = _products.FirstOrDefault((x) => x.Id == id);
            if (product != null)
            {
                return new Response<IProduct> { Succeeded = false, Content = product };
            }

            return new Response<IProduct> { Succeeded = false, Message = "Product could not be found." };
        }
        catch (Exception ex)
        {
            return new Response<IProduct> { Succeeded = false, Message = ex.Message };
        }
    }

    public Response<IProduct> UpdateProductById(string id, Product updatedProduct)
    {
        try
        {

            // har redan check vid inmatning för pris så att det kan parseas, men dubbelcheck här också på att d inte är tomt eller null. 
            if (string.IsNullOrEmpty(updatedProduct.Name) || string.IsNullOrEmpty(updatedProduct.Id) || string.IsNullOrEmpty(updatedProduct.Price.ToString()))
            {
                return new Response<IProduct> { Succeeded = false, Message = $"Could not update product because all required fields was not provided." };
            }

            //om det namnet är unikt eller om namnet används på den egna produkten (sigsjälv typ)
            var existingProduct = _products.FirstOrDefault(x => x.Name.ToLower() == updatedProduct.Name.ToLower() && x.Id != id);
            if (existingProduct != null)
            {
                return new Response<IProduct> { Succeeded = false, Message = $"Product with the name '{updatedProduct.Name}' already exists." };
            }

            var indexToUpdate = _products.FindIndex((x) => x.Id == id);
            if (indexToUpdate > -1)
            {
                _products[indexToUpdate] = updatedProduct;
                var updatedProductsAsString = JsonConvert.SerializeObject(_products);
                var response = _fileService.SaveToFile(updatedProductsAsString);

                if (response.Succeeded)
                {
                    return new Response<IProduct> { Succeeded = true, Message = "Product was successfully updated and saved!" };
                }
                else
                {
                    return new Response<IProduct> { Succeeded = false, Message = "Oops! Some error when saving the updated product." };
                }
            }
            else
            {
                return new Response<IProduct> { Succeeded = false, Message = "The product was not found and could not be updated." };
            }
        }
        catch (Exception ex)
        {
            return new Response<IProduct> { Succeeded = false, Message = ex.Message };
        }
    }

    #region Delete
    public Response<IProduct> DeleteProductById(string id)
    {
        try
        {
            if (_products.Any((x) => x.Id == id))
            {
                if (_products.Remove(_products.FirstOrDefault((x) => x.Id == id)!))
                {
                    var updatedProductListAsJson = JsonConvert.SerializeObject(_products);
                    var response = _fileService.SaveToFile(updatedProductListAsJson);
                    if (response.Succeeded)
                    {
                        return new Response<IProduct> { Succeeded = true, Message = "Product was successfully deleted!" };
                    }
                }
                else
                {
                    return new Response<IProduct> { Succeeded = false, Message = "Something went wrong. Could not delete product." };
                }
            }

            return new Response<IProduct> { Succeeded = false, Message = "Could not find product to remove." };
        }
        catch (Exception ex)
        {
            return new Response<IProduct> { Succeeded = false, Message = ex.Message };

        }
    }


    // om orkar. ska triggas i mainmenu när cat blivit raderad. Då ta bort produkter med det categoriIDt.
    // Annars blir det weird vid visning av cat-name för prods
    public Response<IProduct> DeleteProductsWithSpecificCategoryId(string categoryId)
    {
        try
        {

        } catch (Exception ex)
        {

        }
    }

    #endregion

}
