using Moq;
using Resources.Interfaces;
using Resources.Models;

namespace ShoppingApp.Tests;

public class ProductServiceTests
{

    private readonly Mock<IProductService<Product, IProduct>> _mockProductService = new();

    [Fact]
    public void GetAllProducts__ShouldGetAllProductsFromList_ReturnResponseWithContent()
    {

    }
}
