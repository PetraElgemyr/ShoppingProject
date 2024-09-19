using ShoppingApp.Models;

namespace ShoppingApp.Interfaces;

internal interface ICategoryService
{
    public Response GetAllCategories();
    public Response AddNewCategoryToList(Category categoryRequest);
    public Response UpdateCategoryById(Category updatedCategory);
    public Response DeleteCategoryById(string id);



}
