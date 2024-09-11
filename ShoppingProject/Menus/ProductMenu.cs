using ShoppingProject.Models;
using ShoppingProject.Services;

namespace ShoppingProject.Menus;

internal class ProductMenu
{
    private readonly ExitService _exitService = new ExitService();
    private readonly CategoryService _categoryService = new CategoryService();
    private readonly ProductService _productService = new ProductService();

    internal void ShowProductMenu()
    {
        Console.Clear();
        Console.WriteLine("1. List all products");
        Console.WriteLine("2. Create a new product");
        Console.WriteLine("3. Edit an existing product");
        Console.WriteLine("4. Delete a product");
        Console.WriteLine("0. Exit application");

        Console.WriteLine("Enter an option: ");
        //Fortsätt med readline osv
    }

    internal void CreateProductRequest()
    {
        //Product newProductRequest = new(); gör till request modell eller onödigt?

    }

    internal void EditProductById()
    {

    }

    internal void ListAllProducts()
    {
        var response = _productService.GetAllProducts();

        if (response != null)
        {
            Console.WriteLine("---- PRODUCTS ----");

            foreach (Product product in response)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Description: {product.Description ?? "No description"}");
                Console.WriteLine($"Price: {product.Price}");

                //Kan kolla kategorin med categoryId och hämta, samt skriva ut category name då om orkar
                Console.WriteLine($"Category Id: {product.CategoryId ?? "No selected category"}");

                Console.WriteLine();
            }
        }
    }
}
