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

        } catch (Exception ex)
        {
            Debug.Write(ex.Message);
        }
        return null!;
    }

    internal void AddOrUpdateCategory()
    {

    }

    internal void DeleteCategoryById()
    {
        //Note! Deletes all products with the choosen categoryId too
    }
}
