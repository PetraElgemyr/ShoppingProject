using Moq;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;

namespace ShoppingApp.Tests;

public class ProductServiceTests
{

    private readonly Mock<ProductService> _mockProductService = new();

    [Fact]
    public void GetAllProducts__ShouldGetAllProductsFromList_ReturnResponseWithContent()
    {
        var product = new Product { Name = "TV", CategoryId = "", Price = 5199 };
        var products = new List<Product> { product };

        var expectedRequestResponse = new RequestResponse<IEnumerable<Product>> { Succeeded = Status.Success, Content = products };

        _mockProductService.Setup(productService => productService.GetAllProducts()).Returns(expectedRequestResponse);
        var categoryService = _mockProductService.Object;

        var result = categoryService.GetAllProducts();

        Assert.Equal(Status.Success, result.Succeeded);
        Assert.Equal(products, result.Content);
    }

    [Fact]
    public void GetOneProductById__ShouldGetOneProductById_ReturnResponseWithContent()
    {

    }

    [Fact]
    public void CreateAndAddProductToList__ShouldCreateAndAddProductToList_ReturnResponseWithSuccess()
    {

    }

    [Fact]
    public void UpdateProductById__ShouldUpdateProductById_ReturnResponseWithUpdatedContent()
    {

    }

    [Fact]
    public void DeleteProductById__ShouldDeleteProductById_ReturnResponseWithSuccess()
    {

    }
}
