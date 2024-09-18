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
    }

    internal void HandleCategoryMenuOption(string option)
    {
        if (int.TryParse(option.Trim(), out int selectedOptionNumber))
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
                    GetAndFindCategoryToUpdate();
                    break;
                case 4:
                    GetIdToDeleteAndHandleDeleteRequest();
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
        Console.ReadKey();
    }

    public void ListAllCategories()
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

        Console.Write("Enter category name (required and unique): ");
        newCategory.Name = (Console.ReadLine() ?? "").Trim();

        Console.Write("Enter description (optional): ");
        newCategory.Description = Console.ReadLine() ?? "";

        Response result = _categoryService.AddNewCategoryToList(newCategory);
        if (result != null)
        {
            // TODO göra till if else för succeeded eller ej senare kanske?? 
            Console.WriteLine(result.Message);
        }
    }
    internal void GetAndFindCategoryToUpdate()
    {
        Console.Clear();
        ListAllCategories();
        Response result = _categoryService.GetAllCategories();

        Console.WriteLine("Enter the category ID: ");
        string selectedId = (Console.ReadLine() ?? "").Trim();

        if (result != null)
        {
            IEnumerable<Category> categories = (IEnumerable<Category>)result.Content!;
            var choosenCategory = categories.FirstOrDefault((x) => x.CategoryId.ToLower() == selectedId.ToLower());
            if (choosenCategory != null)
            {
                Console.Clear();
                Console.WriteLine($"ID: {choosenCategory.CategoryId}");
                Console.WriteLine($"Name: {choosenCategory.Name}");
                Console.WriteLine($"Description: {choosenCategory.Description}");
                Console.WriteLine("--------------------");

                Category updatedCategory = choosenCategory;

                Console.WriteLine("Enter new category name (required and unique): ");
                updatedCategory.Name = (Console.ReadLine() ?? "").Trim();

                Console.WriteLine("Enter new description (optional): ");
                updatedCategory.Description = Console.ReadLine() ?? "";

                Response requestResponse = _categoryService.UpdateCategoryById(updatedCategory);
                if (requestResponse != null)
                {
                    // TODO göra till if else för succeeded eller ej senare kanske?? 
                    Console.WriteLine(requestResponse.Message);
                }

            }
        }
    }

    internal void GetIdToDeleteAndHandleDeleteRequest()
    {

        Console.Clear();
        ListAllCategories();
        Response result = _categoryService.GetAllCategories();

        Console.WriteLine("Enter the category ID: ");
        string selectedId = (Console.ReadLine() ?? "").Trim();

        if (!string.IsNullOrEmpty(selectedId))
        {
            Response requestResponse = _categoryService.DeleteCategoryById(selectedId);

            if (requestResponse != null)
            {
                // TODO göra till if else för succeeded eller ej senare kanske?? 
                Console.WriteLine(requestResponse.Message);
            }
        }

    }

}
