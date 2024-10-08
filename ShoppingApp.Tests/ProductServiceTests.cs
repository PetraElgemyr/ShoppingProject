using Moq;
using ShoppingApp.Resources.Services;

namespace ShoppingApp.Tests;

public class ProductServiceTests
{

    private readonly Mock<ProductService> _mockProductService = new();

    [Fact]
    public void GetAllProducts__ShouldGetAllProductsFromList_ReturnResponseWithContent()
    {

    }
}
