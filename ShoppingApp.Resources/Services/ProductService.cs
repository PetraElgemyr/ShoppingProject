using Newtonsoft.Json;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Services;



public class ProductService : IProductService
{
    private List<Product> _products = [];
    private readonly IProductFileService _fileService;

    public ProductService(IProductFileService fileService)
    {
        _fileService = fileService;
    }

    public RequestResponse<Product> CreateAndAddProductToList(Product productRequest)
    {

        try
        {
            var existingProduct = _products.FirstOrDefault(x => x.Name.ToLower().Trim() == productRequest.Name.ToLower().Trim() && x.Id != productRequest.Id);
            if (existingProduct != null)
            {
                return new RequestResponse<Product> { Succeeded = Status.Exists, Message = $"Another Product with the name '{productRequest.Name.Trim()}' already exists." };
            }


            if (string.IsNullOrEmpty(productRequest.Name) || string.IsNullOrEmpty(productRequest.Id) || string.IsNullOrEmpty(productRequest.Price.ToString()) || productRequest.Price <= 0)
            {
                return new RequestResponse<Product> { Succeeded = Status.Failed, Message = $"Could not update product because all required fields (name and price) was not provided correctly." };
            }
            _products.Add(productRequest);

            var productsAsJsonString = JsonConvert.SerializeObject(_products);
            var RequestResponse = _fileService.SaveToFile(productsAsJsonString);

            if (RequestResponse.Succeeded == Status.Success)
            {
                return new RequestResponse<Product> { Succeeded = Status.Success, Message = "Product was successsfully created and list was saved to file :)" };
            }
            else
            {
                return new RequestResponse<Product> { Succeeded = Status.SuccessWithErrors, Message = "Product was created and added to the list but the list could not be saved to file." };
            }
        }
        catch (Exception ex)
        {
            return new RequestResponse<Product> { Succeeded = Status.Failed, Message = ex.Message };
        }
    }
    public RequestResponse<IEnumerable<Product>> GetAllProducts()
    {
        try
        {
            var result = _fileService.GetFromFile();

            if (result.Succeeded == Status.Success && !string.IsNullOrEmpty(result.Content))
            {
                _products = JsonConvert.DeserializeObject<List<Product>>(result.Content!)!;
                return new RequestResponse<IEnumerable<Product>> { Succeeded = Status.Success, Content = _products };
            }
            else
            {
                return new RequestResponse<IEnumerable<Product>> { Succeeded = Status.Failed, Message = "Could not fetch products from file." };
            }
        }
        catch (Exception ex)
        {
            return new RequestResponse<IEnumerable<Product>> { Succeeded = Status.Failed, Message = ex.Message };
        }
    }
    public RequestResponse<Product> GetOneProductById(string id)
    {
        try
        {
            var product = _products.FirstOrDefault((x) => x.Id == id);
            if (product != null)
            {
                return new RequestResponse<Product> { Succeeded = Status.Success, Content = product };
            }

            return new RequestResponse<Product> { Succeeded = Status.NotFound, Message = "Product could not be found." };
        }
        catch (Exception ex)
        {
            return new RequestResponse<Product> { Succeeded = Status.Failed, Message = ex.Message };
        }
    }

    public RequestResponse<Product> UpdateProductById(string id, Product updatedProduct)
    {
        try
        {

            // har redan check vid inmatning för pris så att det kan parseas, men dubbelcheck här också på att d inte är tomt eller null. 
            if (string.IsNullOrEmpty(updatedProduct.Name) || string.IsNullOrEmpty(updatedProduct.Id) || string.IsNullOrEmpty(updatedProduct.Price.ToString()) || updatedProduct.Price <= 0)
            {
                return new RequestResponse<Product> { Succeeded = Status.Failed, Message = $"Could not update product because all required fields was not provided correctly." };
            }

            //om det namnet är unikt eller om namnet används på den egna produkten (sigsjälv typ)
            var existingProduct = _products.FirstOrDefault(x => x.Name.ToLower().Trim() == updatedProduct.Name.ToLower().Trim() && x.Id != id);
            if (existingProduct != null)
            {
                return new RequestResponse<Product> { Succeeded = Status.Exists, Message = $"Product with the name '{updatedProduct.Name.Trim()}' already exists." };
            }

            var indexToUpdate = _products.FindIndex((x) => x.Id == id);
            if (indexToUpdate > -1)
            {
                _products[indexToUpdate] = updatedProduct;
                var updatedProductsAsString = JsonConvert.SerializeObject(_products);
                var RequestResponse = _fileService.SaveToFile(updatedProductsAsString);

                if (RequestResponse.Succeeded == Status.Success)
                {
                    return new RequestResponse<Product> { Succeeded = Status.Success, Message = "Product was updated and the list was successfully saved to file!", Content = updatedProduct };
                }
                else
                {
                    return new RequestResponse<Product> { Succeeded = Status.SuccessWithErrors, Message = "Oops! Product was updated in the list but the list could not be saved to file." };
                }
            }
            else
            {
                return new RequestResponse<Product> { Succeeded = Status.NotFound, Message = "The product was not found and could not be updated." };
            }
        }
        catch (Exception ex)
        {
            return new RequestResponse<Product> { Succeeded = Status.Failed, Message = ex.Message };
        }
    }

    #region Delete
    public RequestResponse<Product> DeleteProductById(string id)
    {
        try
        {
            if (_products.Any((x) => x.Id == id))
            {
                if (_products.Remove(_products.FirstOrDefault((x) => x.Id == id)!))
                {
                    var updatedProductListAsJsonString = JsonConvert.SerializeObject(_products);
                    var RequestResponse = _fileService.SaveToFile(updatedProductListAsJsonString);
                    if (RequestResponse.Succeeded == Status.Success)
                    {
                        return new RequestResponse<Product> { Succeeded = Status.Success, Message = "Product was successfully deleted and the list was saved!" };
                    }
                }
                else
                {
                    return new RequestResponse<Product> { Succeeded = Status.Failed, Message = "Something went wrong. Could not delete product." };
                }
            }

            return new RequestResponse<Product> { Succeeded = Status.NotFound, Message = "Could not find product to remove." };
        }
        catch (Exception ex)
        {
            return new RequestResponse<Product> { Succeeded = Status.Failed, Message = ex.Message };

        }
    }








    

    public RequestResponse<Product> DeleteProductsWithSpecificCategoryId(string categoryId)
    {
        try
        {
            if (String.IsNullOrEmpty(categoryId))
            {
                // kasta fel
                return new RequestResponse<Product> { Succeeded = Status.Failed, Message = $"No category ID was provided." };

            }

            int amountOfDeletedProducts = _products.RemoveAll((x) => x.CategoryId == categoryId);

            if (amountOfDeletedProducts <= 0)
            {
                return new RequestResponse<Product> { Succeeded = Status.NotFound, Message = $"No categories with id: {categoryId} was found." };
            }

            var updatedProductListAsJsonString = JsonConvert.SerializeObject(_products);
            var RequestResponse = _fileService.SaveToFile(updatedProductListAsJsonString);

            if (RequestResponse.Succeeded == Status.Success && amountOfDeletedProducts > 0)
            {
                return new RequestResponse<Product> { Succeeded = Status.Success, Message = "Products was successfully deleted from the list and the list was saved!" };
            }
            else if (RequestResponse.Succeeded != Status.Success && amountOfDeletedProducts > 0)
            {
                return new RequestResponse<Product> { Succeeded = Status.SuccessWithErrors, Message = "Found products was successfully deleted from the list but the list could not be saved to file." };
            }
            else
            {
                return new RequestResponse<Product> { Succeeded = Status.Failed, Message = "Something went wrong. Could not delete products." };
            }


        }
        catch (Exception ex)
        {
            return new RequestResponse<Product> { Succeeded = Status.Failed, Message = ex.Message };

        }
    }

    #endregion

}
