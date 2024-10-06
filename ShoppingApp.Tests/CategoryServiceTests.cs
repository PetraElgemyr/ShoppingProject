using Moq;
using Resources.Enums;
using Resources.Interfaces;
using Resources.Models;

namespace ShoppingApp.Tests;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryService<Category, ICategory>> _mockCategoryService = new();

    [Fact]
    public void GetAllCategories__ShouldGetAllCategoriesFromList_ReturnResponseWithContent()
    {
        var category = new Category { Name = "Electronics" };
        var categories = new List<Category> { category };

        var expectedResponse = new Response<IEnumerable<ICategory>> { Succeeded = Status.Success, Content = categories };

        _mockCategoryService.Setup(categoryService => categoryService.GetAllCategories()).Returns(expectedResponse);
        var categoryService = _mockCategoryService.Object;

        var response = categoryService.GetAllCategories();

        Assert.Equal(Status.Success, response.Succeeded);
        Assert.Equal(categories, response.Content);
    }

    [Fact]
    public void CreateAndAddCategoryToList__ShouldReturnSuccess_WhenCategoryIsAddedToList()
    {
        var category = new Category { Name = "Kitchen" };
        var expectedResponse = new Response<ICategory> { Succeeded = Status.Success, Message = "Category was successfully created and saved to file! :)" };

        _mockCategoryService.Setup(categoryService => categoryService.CreateAndAddCategoryToList(category)).Returns(expectedResponse);
        var categoryService = _mockCategoryService.Object;

        var response = categoryService.CreateAndAddCategoryToList(category);

        Assert.Equal(Status.Success, response.Succeeded);
        Assert.Equal(expectedResponse.Message, response.Message);
    }

    [Fact]
    public void CreateAndAddCategoryToList__ShouldReturnStatusExists_WhenAddingDuplicateToList()
    {
        var category = new Category { Name = "Kitchen" };
        var expectedResponse = new Response<ICategory> { Succeeded = Status.Exists, Message = $"Another category with the name '{category.Name}' already exists." };

        _mockCategoryService.Setup(categoryService => categoryService.CreateAndAddCategoryToList(category)).Returns(expectedResponse);
        var categoryService = _mockCategoryService.Object;

        // Lägg till första
        categoryService.CreateAndAddCategoryToList(category);

        var response = categoryService.CreateAndAddCategoryToList(category);


        Assert.Equal(Status.Exists, response.Succeeded);
        Assert.Equal(expectedResponse.Message, response.Message);
    }

    [Fact]
    public void GetOneCategoryById__ShouldReturnOneCategory_WhenCategoryExists() {
        var categeory = new Category { Id = Guid.NewGuid().ToString(), Name = "Bedroom" };
        var expectedResponse = new Response<ICategory> { Succeeded = Status.Success, Content = categeory };

        _mockCategoryService.Setup(categoryService => categoryService.GetOneCategoryById(categeory.Id)).Returns(expectedResponse);
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
        var expectedResponse = new Response<ICategory> { Succeeded = Status.Success, Message= "Category was successfully updated and saved to the file!", Content = updatedCategory };


        _mockCategoryService.Setup(categoryService => categoryService.UpdateCategoryById(categeory.Id, updatedCategory)).Returns(expectedResponse);
        var categoryService = _mockCategoryService.Object;

        var result = categoryService.UpdateCategoryById(id, updatedCategory);

        Assert.Equal(Status.Success, result.Succeeded);
        Assert.Equal(expectedResponse.Message, result.Message);
        Assert.NotEqual(categeory, result.Content);
    }

    [Fact]
    public void DeleteCategoryById__ShouldReturnSuccessResult_WhenCategoryIsDeleted()
    {
        var id = Guid.NewGuid().ToString();
        var categeory = new Category { Id = id, Name = "Bathroom" };

        var expectedResponse =   new Response<ICategory> { Succeeded = Status.Success, Message = "Category was successfully deleted and list was saved to file!" };


        _mockCategoryService.Setup(categoryService => categoryService.DeleteCategoryById(id)).Returns(expectedResponse);
        var categoryService = _mockCategoryService.Object;

        var result = categoryService.DeleteCategoryById(id);

        Assert.Equal(Status.Success, result.Succeeded);
        Assert.Equal(expectedResponse.Message, result.Message);

    }







    /*
     	

    */
}
