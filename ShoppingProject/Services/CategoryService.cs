using ShoppingProject.Models;
using System.Diagnostics;

namespace ShoppingProject.Services;

internal class CategoryService
{
    private List<Category> categories = [];
    internal IEnumerable<Category> GetAllCategories()
    {
        try
        {
            return categories;
        }
        catch (Exception ex)
        {
            Debug.Write(ex.Message);
        }
        return null!;
    }

    internal void AddOrUpdateCategory(Category categoryRequest)
    {

    }

    internal void DeleteCategoryById(string id)
    {
        //Note! Deletes all products with the choosen categoryId too
    }
}
