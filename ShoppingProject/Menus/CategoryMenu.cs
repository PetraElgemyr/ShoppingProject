using ShoppingApp.Models;
using ShoppingApp.Services;

namespace ShoppingApp.Menus;

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
        HandleCategoryMenuOption(Console.ReadLine() ?? "");
        Console.ReadKey();
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
                    ListAllCategories();
                    break;
                case 2:
                    CreateCategoryRequest();
                    break;
                case 3:
                    string? idToUpdate = GetAndFindCategoryByIdFromUser();
                    if (idToUpdate != null)
                        _categoryService.UpdateCategoryById(  );
                    break;
                case 4:
                    string? idToDelete = GetAndFindCategoryByIdFromUser();
                    if (idToDelete != null)
                        _categoryService.DeleteCategoryById(idToDelete);
                    break;
                default:
                    Console.WriteLine("Invalid option selected");
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
        Response result = _categoryService.GetAllCategories();

        if (result != null && result.Content != null)
        {
            var categories = (IEnumerable<Category>)result.Content;

            if (categories.Count() > 0)
            {
                Console.WriteLine("---- CATEGORIES ----");
                foreach (Category category in categories)
                {
                    Console.WriteLine($"Id: {category.CategoryId}");
                    Console.WriteLine($"Name: {category.Name}");
                    Console.WriteLine($"Description: {category.Description}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No categories found.");

            }
        }
        else
        {
            Console.WriteLine($"Something went wrong.");
        }
    }

    internal void CreateCategoryRequest()
    {
        Console.Clear();
        Category newCategory = new();

        Console.WriteLine("--- CREATE CATEGORY ---");

        Console.Write("Enter category name: ");
        newCategory.Name = Console.ReadLine() ?? "";

        Console.Write("Enter description: ");
        newCategory.Description = Console.ReadLine() ?? "";

        Response result = _categoryService.AddNewCategoryToList(newCategory);
        if (result != null)
        {
            // TODO göra till if else för succeeded eller ej senare kanske?? 
            Console.WriteLine(result.Message);
        }
    }
    internal string? GetAndFindCategoryByIdFromUser()
    {
        Console.Clear();
        ListAllCategories();
        Response result = _categoryService.GetAllCategories();
        Console.WriteLine("Enter the id for the category to edit");
        string selectedId = Console.ReadLine() ?? "";

        if (selectedId != null && selectedId.Length > 0)
        {
            return selectedId;
        }
        return null;
    }

}
