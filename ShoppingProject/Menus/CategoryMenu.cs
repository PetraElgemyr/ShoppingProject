using ShoppingProject.Models;
using ShoppingProject.Services;

namespace ShoppingProject.Menus;

internal class CategoryMenu
{
    private readonly ExitService _exitService = new ExitService();
    private readonly CategoryService _categoryService = new CategoryService();
    private readonly ProductService _productService = new ProductService();
    internal void ShowCategoryMenu()
    {
        Console.Clear();
        Console.WriteLine("1. List all categorys");
        Console.WriteLine("2. Create a new category");
        Console.WriteLine("3. Edit an existing category");
        Console.WriteLine("4. Delete a category and related products");
        Console.WriteLine("0. Exit application");

        Console.WriteLine("Enter an option: ");
        //Console.ReadLine() ?? ""
    }

    public void HandleCategoryMenuOption(string option)
    {
        if (int.TryParse(option, out int selectedOptionNumber))
        {
            switch (selectedOptionNumber)
            {
                case 0:
                    _exitService.ExitApplication();
                    break;
                case 1:
                    CreateCategoryRequest();
                    break;
                case 2:

                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid option selected");
        }
    }



    internal void ListAllCategories()
    {
        var response = _categoryService.GetAllCategories();
        if (response != null)
        {
            Console.WriteLine("---- CATEGORIES ----");

            foreach (Category category in response)
            {
                Console.WriteLine($"Id: {category.CategoryId}");
                Console.WriteLine($"Name: {category.Name}");
                Console.WriteLine($"Description: {category.Description}");
                Console.WriteLine();
            }
        }
    }

    internal void CreateCategoryRequest()
    {

    }

    internal void EditCategoryById(string id)
    {
        ListAllCategories();
        Console.WriteLine("Enter the id for the category to edit");
        string selectedId = Console.ReadLine() ?? "";

        if(selectedId != null && selectedId.Length > 0)
        {
           
        }
    }
}
