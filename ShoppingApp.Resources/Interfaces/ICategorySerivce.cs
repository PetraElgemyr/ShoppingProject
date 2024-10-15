using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Interfaces;

public interface ICategoryService
{
    RequestResponse<Category> CreateAndAddCategoryToList(Category categoryRequest);
    RequestResponse<Category> DeleteCategoryById(string id);
    RequestResponse<IEnumerable<Category>> GetAllCategories();
    RequestResponse<Category> GetOneCategoryById(string id);
    RequestResponse<Category> UpdateCategoryById(string id, Category updatedCategory);
}


