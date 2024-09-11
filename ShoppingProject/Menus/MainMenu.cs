using ShoppingProject.Services;

namespace ShoppingProject.Menus;

internal class MainMenu
{

    private readonly ExitService _exitService =  new ExitService();
    private readonly CategoryMenu _categoryMenu = new CategoryMenu();
    private readonly ProductMenu _productMenu = new ProductMenu();

    public void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("1. View the category menu");
        Console.WriteLine("2. View the product menu");
        Console.WriteLine("0. Exit application");

        Console.WriteLine("Enter an option: ");
        HandleMainMenuOption(Console.ReadLine() ?? "");
    }

    public void HandleMainMenuOption(string option)
    {
        if (int.TryParse(option, out int selectedOptionNumber))
        {
            switch (selectedOptionNumber)
            {
                case 0:
                    _exitService.ExitApplication();
                    break;
                case 1:
                    _categoryMenu.ShowCategoryMenu();
                    break;
                case 2:
                    _productMenu.ShowProductMenu();
                    break;
                default:
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid option selected");
        }
    }




}
