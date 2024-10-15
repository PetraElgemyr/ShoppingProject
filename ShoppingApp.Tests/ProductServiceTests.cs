using Moq;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Models;

namespace ShoppingApp.Tests;

public class ProductServiceTests
{

    private readonly Mock<IProductService> _mockProductService = new();

    [Fact]
    public void GetAllProducts__ShouldGetAllProductsFromList_ReturnResponseWithContent()
    {
        var product = new Product { Name = "TV", CategoryId = "", Price = 5199 };
        var secondProduct = new Product { Name = "Table", CategoryId = "", Price = 899 };

        List<Product> products = [product, secondProduct];

        var expectedRequestResponse = new RequestResponse<IEnumerable<Product>> { Succeeded = Status.Success, Content = products };

        _mockProductService.Setup(prodService => prodService.GetAllProducts()).Returns(expectedRequestResponse);
        var productService = _mockProductService.Object;

        var result = productService.GetAllProducts();

        Assert.Equal(Status.Success, result.Succeeded);
        Assert.Equal(products, result.Content);
        Assert.Equal(result.Content!.Count(), products.Count());
    }

    [Fact]
    public void GetOneProductById__ShouldGetOneProductById_ReturnResponseWithContent()
    {
        var id = Guid.NewGuid().ToString();
        var product = new Product {Id = id, Name = "TV", CategoryId = "", Price = 5199 };
        var expectedRequestResponse = new RequestResponse<Product> { Succeeded = Status.Success, Content = product };

        _mockProductService.Setup((prodService) => prodService.GetOneProductById(id)).Returns(expectedRequestResponse);
        var productService = _mockProductService.Object;

        var result = productService.GetOneProductById(id);

        Assert.Equal(Status.Success, result.Succeeded);
        Assert.Equal(product, result.Content);
    }

    [Fact]
    public void CreateAndAddProductToList__ShouldCreateAndAddProductToList_ReturnResponseWithSuccess()
    {
        var product = new Product { Name = "TV", CategoryId = "", Price = 5199 };
        var expectedRequestResponse = new RequestResponse<Product> { Succeeded = Status.Success, Message = "Product was successsfully created and list was saved to file :)" };

        _mockProductService.Setup(productService => productService.CreateAndAddProductToList(product)).Returns(expectedRequestResponse);

        var productService = _mockProductService.Object;    
        var result = productService.CreateAndAddProductToList(product);

        Assert.Equal(Status.Success, result.Succeeded );
        Assert.Equal(expectedRequestResponse.Message, result.Message);
    }

    [Fact]
    public void CreateAndAddProductToList__ShouldTryToAddDuplicateToList_ReturnSuccededAsExists()
    {
        var product = new Product { Name = "TV", CategoryId = "", Price = 5199 };
        var expectedRequestResponse = new RequestResponse<Product> { Succeeded = Status.Exists, Message = $"Another Product with the name '{product.Name}' already exists." };

        _mockProductService.Setup(productService => productService.CreateAndAddProductToList(product)).Returns(expectedRequestResponse);

        var productService = _mockProductService.Object;
        productService.CreateAndAddProductToList(product);

        var result = productService.CreateAndAddProductToList(product);

        Assert.Equal(Status.Exists, result.Succeeded);
        Assert.Equal(expectedRequestResponse.Message, result.Message);
    }

    [Fact]
    public void UpdateProductById__ShouldUpdateProductById_ReturnResponseWithUpdatedContent()
    {
        var id = Guid.NewGuid().ToString();
        var product = new Product { Id = id, Name = "TV", CategoryId = "", Price = 3199 };
        var updatedProduct = new Product { Id = id, Name = "SmartTV", CategoryId = "", Price = 7999 };
        var expectedRequestResponse = new RequestResponse<Product> { Succeeded = Status.Success, Message = "Product was updated and the list was successfully saved to file!" };
        _mockProductService.Setup(productService => productService.UpdateProductById(id, updatedProduct)).Returns(expectedRequestResponse); 

        var productService = _mockProductService.Object;
        var result = productService.UpdateProductById(id, updatedProduct);

        Assert.Equal(Status.Success, result.Succeeded );
        Assert.Equal(expectedRequestResponse.Message, result.Message);
        Assert.NotEqual(product, result.Content);
    }

    [Fact]
    public void DeleteProductById__ShouldDeleteProductById_ReturnResponseWithSuccess()
    {
        var id = Guid.NewGuid().ToString();
        var product = new Product { Id = id, Name = "TV", CategoryId = "", Price = 3199 };


        var expectedRequestResponse = new RequestResponse<Product> { Succeeded = Status.Success, Message = "Product was successfully deleted and the list was saved!" };


        _mockProductService.Setup(productService => productService.DeleteProductById(id)).Returns(expectedRequestResponse);
        var productService = _mockProductService.Object;

        var result = productService.DeleteProductById(id);

        Assert.Equal(Status.Success, result.Succeeded);
        Assert.Equal(expectedRequestResponse.Message, result.Message);
    }
}
