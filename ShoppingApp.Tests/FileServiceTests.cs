using Moq;
using Newtonsoft.Json;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Models;

namespace ShoppingApp.Tests;

public class FileServiceTests
{
    private readonly Mock<IFileService> _mockFileService = new();
    [Fact]
    public void GetFromFile__ShouldGetProductsFromFile_ReturnResponseWithContent()
    {
        var product = new Product { Name = "TV", CategoryId = "", Price = 5199 };
        var products = new List<Product> { product };
       
        var expectedRequestResponse = new RequestResponse<string> { Succeeded = Status.Success, Content = products.ToString() };
        _mockFileService.Setup((fileService) => fileService.GetFromFile()).Returns(expectedRequestResponse);

        var fileService = _mockFileService.Object;
        var result = fileService.GetFromFile();

        Assert.Equal( Status.Success, result.Succeeded);
        Assert.Equal(expectedRequestResponse.Content, result.Content);
    }

    [Fact]
    public void GetFromFile__ShouldGetCategoriesFromFile_ReturnResponseWithContent()
    {
        var category = new Category { Name = "Electronics" };
        var categories = new List<Category> { category };

        var expectedRequestResponse = new RequestResponse<string> { Succeeded = Status.Success, Content = categories.ToString() };
        _mockFileService.Setup((fileService) => fileService.GetFromFile()).Returns(expectedRequestResponse);

        var fileService = _mockFileService.Object;
        var result = fileService.GetFromFile();

        Assert.Equal(Status.Success, result.Succeeded );
        Assert.Equal(expectedRequestResponse.Content, result.Content);
    }

    [Fact]
    public void SaveToFile__ShouldSaveCategoriesToFile_ReturnResponseWithSuccess()
    {
        var product = new Product { Name = "TV", CategoryId = "", Price = 5199 };
        var products = new List<Product> { product };
        var productsAsJsonString = JsonConvert.SerializeObject(products);


        var expectedRequestResponse = new RequestResponse<string> { Succeeded = Status.Success };
        _mockFileService.Setup((fileService) => fileService.SaveToFile(productsAsJsonString)).Returns(expectedRequestResponse);

        var fileService = _mockFileService.Object;
        var result = fileService.SaveToFile(productsAsJsonString);

        Assert.Equal(Status.Success, result.Succeeded);
    }

    [Fact]
    public void SaveToFile__ShouldSaveProductsToFile_ReturnResponseWithSuccess()
    {
        var category = new Category { Name = "Bathroom"};
        var categories = new List<Category> { category };
        var categoriesAsJsonString = JsonConvert.SerializeObject(categories);


        var expectedRequestResponse = new RequestResponse<string> { Succeeded = Status.Success };
        _mockFileService.Setup((fileService) => fileService.SaveToFile(categoriesAsJsonString)).Returns(expectedRequestResponse);

        var fileService = _mockFileService.Object;
        var result = fileService.SaveToFile(categoriesAsJsonString);

        Assert.Equal(Status.Success, result.Succeeded);
    }

   
}
