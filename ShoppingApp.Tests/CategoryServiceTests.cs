using Moq;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;

namespace ShoppingApp.Tests;

public class CategoryServiceTests
{
    private readonly Mock<CategoryService> _mockCategoryService = new();

    [Fact]
    public void GetAllCategories__ShouldGetAllCategoriesFromList_ReturnRequestResponseWithContent()
    {
        var category = new Category { Name = "Electronics" };
        var categories = new List<Category> { category };

        var expectedRequestResponse = new RequestResponse<IEnumerable<Category>> { Succeeded = Status.Success, Content = categories };

        _mockCategoryService.Setup(categoryService => categoryService.GetAllCategories()).Returns(expectedRequestResponse);
        var categoryService = _mockCategoryService.Object;

        var requestResponse = categoryService.GetAllCategories();

        Assert.Equal(Status.Success, requestResponse.Succeeded);
        Assert.Equal(categories, requestResponse.Content);
    }

    [Fact]
    public void CreateAndAddCategoryToList__ShouldReturnSuccess_WhenCategoryIsAddedToList()
    {
        var category = new Category { Name = "Kitchen" };
        var expectedRequestResponse = new RequestResponse<Category> { Succeeded = Status.Success, Message = "Category was successfully created and saved to file! :)" };

        _mockCategoryService.Setup(categoryService => categoryService.CreateAndAddCategoryToList(category)).Returns(expectedRequestResponse);
        var categoryService = _mockCategoryService.Object;

        var requestResponse = categoryService.CreateAndAddCategoryToList(category);

        Assert.Equal(Status.Success, requestResponse.Succeeded);
        Assert.Equal(expectedRequestResponse.Message, requestResponse.Message);
    }

    [Fact]
    public void CreateAndAddCategoryToList__ShouldReturnStatusExists_WhenAddingDuplicateToList()
    {
        var category = new Category { Name = "Kitchen" };
        var expectedRequestResponse = new RequestResponse<Category> { Succeeded = Status.Exists, Message = $"Another category with the name '{category.Name}' already exists." };

        _mockCategoryService.Setup(categoryService => categoryService.CreateAndAddCategoryToList(category)).Returns(expectedRequestResponse);
        var categoryService = _mockCategoryService.Object;

        // Lägg till första
        categoryService.CreateAndAddCategoryToList(category);

        var requestResponse = categoryService.CreateAndAddCategoryToList(category);


        Assert.Equal(Status.Exists, requestResponse.Succeeded);
        Assert.Equal(expectedRequestResponse.Message, requestResponse.Message);
    }

    [Fact]
    public void GetOneCategoryById__ShouldReturnOneCategory_WhenCategoryExists() {
        var categeory = new Category { Id = Guid.NewGuid().ToString(), Name = "Bedroom" };
        var expectedRequestResponse = new RequestResponse<Category> { Succeeded = Status.Success, Content = categeory };

        _mockCategoryService.Setup(categoryService => categoryService.GetOneCategoryById(categeory.Id)).Returns(expectedRequestResponse);
        var categoryService = _mockCategoryService.Object;

        var result = categoryService.GetOneCategoryById(categeory.Id);

        Assert.Equal(Status.Success, result.Succeeded );
        Assert.Equal(categeory, result.Content);    

    }

    [Fact]
    public void UpdateCategoryById__ShouldReturnUpdatedCategory_WhenCategoryIsUpdated()
    {
        var id = Guid.NewGuid().ToString();
        var categeory = new Category { Id = id, Name = "Balcony" };
        var updatedCategory = new Category { Id = id, Name = "Outdoor and balcony" };
        var expectedRequestResponse = new RequestResponse<Category> { Succeeded = Status.Success, Message= "Category was successfully updated and saved to the file!", Content = updatedCategory };


        _mockCategoryService.Setup(categoryService => categoryService.UpdateCategoryById(categeory.Id, updatedCategory)).Returns(expectedRequestResponse);
        var categoryService = _mockCategoryService.Object;

        var result = categoryService.UpdateCategoryById(id, updatedCategory);

        Assert.Equal(Status.Success, result.Succeeded);
        Assert.Equal(expectedRequestResponse.Message, result.Message);
        Assert.NotEqual(categeory, result.Content);
    }

    [Fact]
    public void DeleteCategoryById__ShouldReturnSuccessResult_WhenCategoryIsDeleted()
    {
        var id = Guid.NewGuid().ToString();
        var categeory = new Category { Id = id, Name = "Bathroom" };

        var expectedRequestResponse =   new RequestResponse<Category> { Succeeded = Status.Success, Message = "Category was successfully deleted and list was saved to file!" };


        _mockCategoryService.Setup(categoryService => categoryService.DeleteCategoryById(id)).Returns(expectedRequestResponse);
        var categoryService = _mockCategoryService.Object;

        var result = categoryService.DeleteCategoryById(id);

        Assert.Equal(Status.Success, result.Succeeded);
        Assert.Equal(expectedRequestResponse.Message, result.Message);

    }







    /*
     	

    */
}
