using Resources.Interfaces;
using Resources.Models;
using Resources.Services;

namespace ShoppingApp.Menus;

internal class MainMenu
{
    private readonly ICategoryService<Category, ICategory> _categoryService = new CategoryService(@"C:\Skola\Nackademin\Programmering-sharp\ShoppingProject\categories.json");
    private readonly IProductService<Product, IProduct> _productService = new ProductService(@"C:\Skola\Nackademin\Programmering-sharp\ShoppingProject\products.json");

    public void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("0. Exit application");

        Console.WriteLine("\n----CATEGORY MENU----");
        Console.WriteLine("1. Create a new category");
        Console.WriteLine("2. List all categorys");
        Console.WriteLine("3. Update an existing category");
        Console.WriteLine("4. Delete a category and related products");

        Console.WriteLine("\n----PRODUCT MENU----");
        Console.WriteLine("5. Create a new product");
        Console.WriteLine("6. List all products");
        Console.WriteLine("7. Update an existing product");
        Console.WriteLine("8. Delete a product");


        Console.WriteLine("Enter an option: ");
        HandleMenuOption(Console.ReadLine() ?? "");
    }

    public void HandleMenuOption(string option)
    {
        if (int.TryParse(option.Trim(), out int selectedOptionNumber))
        {
            switch (selectedOptionNumber)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    CreateCategoryRequest(); // create c
                    break;
                case 2:
                    ListAllCategories(); // read c
                    break;
                case 3:
                    GetAndFindCategoryToUpdate(); // update c
                    break;
                case 4:
                    GetIdToDeleteAndHandleDeleteCategoryRequest(); // delete c
                    break;
                case 5:
                    CreateProductRequest(); // create p
                    break;
                case 6:
                    ListAllProducts(); // read p
                    break;
                case 7:
                    GetAndFindProductToUpdate(); // update p
                    break;
                case 8:
                    GetIdToDeleteAndHandleDeleteProductRequest(); // delete p
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
        Console.Write("\nPress any key to continue.");
        Console.ReadKey();
    }

    #region Category

    //CREATE CATEGORY
    internal void CreateCategoryRequest()
    {
        Console.Clear();
        Category newCategory = new();

        Console.WriteLine("--- CREATE CATEGORY ---");

        Console.WriteLine("Enter category name (required and unique): ");
        newCategory.Name = (Console.ReadLine() ?? "").Trim();

        Console.WriteLine("Enter description (optional): ");
        newCategory.Description = Console.ReadLine() ?? "";

        Response<ICategory> result = _categoryService.CreateAndAddCategoryToList(newCategory);
        if (result != null)
        {
            // TODO göra till if else för succeeded eller ej senare kanske?? 
            Console.WriteLine(result.Message);
        }
    }

    // READ CATEGORIES
    public void ListAllCategories()
    {
        Response<IEnumerable<ICategory>> result = _categoryService.GetAllCategories();

        if (result != null)
        {
            if (result.Content!.Any())
            {
                Console.WriteLine("---- CATEGORIES ----");
                foreach (Category category in result.Content!)
                {
                    Console.WriteLine($"\nId: {category.Id}");
                    Console.WriteLine($"Name: {category.Name}");
                    Console.WriteLine($"Description: {category.Description}");
                
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

    // UPDATE CATEGORY
    internal void GetAndFindCategoryToUpdate()
    {
        Console.Clear();
        ListAllCategories();

        Console.WriteLine("Enter the category ID: ");
        string selectedId = (Console.ReadLine() ?? "").Trim();
        if (!string.IsNullOrEmpty(selectedId))
        {
            Response<ICategory> result = _categoryService.GetOneCategoryById(selectedId);

            if (result != null && result.Content != null && result.Content!.Id == selectedId)
            {
                if (result.Content != null)
                {
                    Console.Clear();
                    Console.WriteLine("---- CATEGORY TO UPDATE ----");
                    Console.WriteLine($"ID: {result.Content.Id}");
                    Console.WriteLine($"Name: {result.Content.Name}");
                    Console.WriteLine($"Description: {result.Content.Description}");

                    Console.WriteLine("-----------------------");

                    Category categoryToUpdate = new Category { Id = result.Content.Id };

                    Console.WriteLine("Enter new category name (required and unique): ");
                    categoryToUpdate.Name = (Console.ReadLine() ?? "").Trim();

                    Console.WriteLine("Enter new description (optional): ");
                    categoryToUpdate.Description = Console.ReadLine() ?? "";

                    Response<ICategory> requestResponse = _categoryService.UpdateCategoryById(selectedId, categoryToUpdate);
                    if (requestResponse != null)
                    {
                        // TODO göra till if else för succeeded eller ej senare kanske?? 
                        Console.WriteLine(requestResponse.Message);
                    }

                }
            }
            else
            {
                Console.WriteLine("Something went wrong when fetching the category to update.");
            }
        }
        else
        {
            Console.WriteLine("You have to provide an ID.");
        }
    }

    // DELETE CATEGORY
    internal void GetIdToDeleteAndHandleDeleteCategoryRequest()
    {

        Console.Clear();
        ListAllCategories();
        Response<IEnumerable<ICategory>> result = _categoryService.GetAllCategories();

        if (result != null)
        {
            Console.WriteLine("Enter the category ID: ");
            string selectedId = (Console.ReadLine() ?? "").Trim();

            if (!string.IsNullOrEmpty(selectedId))
            {
                Response<ICategory> requestResponse = _categoryService.DeleteCategoryById(selectedId);

                if (requestResponse != null)
                {
                    // TODO göra till if else för succeeded eller ej senare kanske?? 
                    Console.WriteLine(requestResponse.Message);
                }
            }
        }



    }


    #endregion

    #region Product
    // CREATE PRODUCT
    internal void CreateProductRequest()
    {
        // TODO Product newProductRequest = new(); gör till request modell eller onödigt?
        Console.Clear();
        Product newProduct = new();

        Console.WriteLine("--- CREATE PRODUCT ---");

        Console.WriteLine("Enter product name (required and unique): ");
        newProduct.Name = (Console.ReadLine() ?? "").Trim();

        Console.WriteLine("Enter description (optional): ");
        newProduct.Description = Console.ReadLine() ?? "";

        Console.WriteLine("Enter the product's price (required, min 1):");
        do
        {
            if (int.TryParse((Console.ReadLine() ?? "").Trim(), out int price) && price > 0)
            {
                newProduct.Price = price;
            }
            else
            {
                Console.WriteLine("Incorrect price. Please try again.");
            }
        } while (newProduct.Price < 1);

        // TODO kolla om shared class library kansk kan lösa?? referera till service dit för att komma åt categories?.
        Response<IEnumerable<ICategory>> categoryResponse = _categoryService.GetAllCategories();

        //Bara om det finns kategorier ska man fråga om categori-id för nya produkten
        if (categoryResponse != null && categoryResponse.Content != null && categoryResponse.Content!.Any())
        {
            ListAllCategories();
            Console.WriteLine("\nEnter the product's new category ID (optional): ");
            string categoryIdInput = "";
            do
            {
                categoryIdInput = (Console.ReadLine() ?? "").Trim();
                if (categoryResponse.Content.Any((x) => x.Id == categoryIdInput || string.IsNullOrEmpty(categoryIdInput)))
                {
                    newProduct.CategoryId = categoryIdInput;
                
                }
                else if (!string.IsNullOrEmpty(categoryIdInput) && !categoryResponse.Content.Any((x) => x.Id == categoryIdInput))
                {
                    Console.WriteLine("Incorrect category ID. Please try again.");

                }
            } while (newProduct.CategoryId != categoryIdInput);
        }


        Response<IProduct> result = _productService.CreateAndAddProductToList(newProduct);
        if (result != null)
        {
            // TODO göra till if else för succeeded eller ej senare kanske?? 
            Console.WriteLine(result.Message);
        }
    }


    // READ PRODUCTS
    internal void ListAllProducts()
    {
        Response<IEnumerable<IProduct>> productResult = _productService.GetAllProducts();
        Response<IEnumerable<ICategory>> categoryResponse = _categoryService.GetAllCategories();

        if (productResult != null && productResult.Content != null)
        {
            var products = productResult.Content;

            if (products.Any())
            {
                Console.WriteLine("---- PRODUCTS ----");
                foreach (Product p in products)
                {
                    Console.WriteLine($"\nId: {p.Id}");
                    Console.WriteLine($"Name: {p.Name}");
                    Console.WriteLine($"Description: {p.Description ?? "No description"}");
                    Console.WriteLine($"Price: {p.Price}");

                    if (categoryResponse.Content != null && categoryResponse.Content.Any((x) => x.Id == p.CategoryId))
                    {
                        var categoyName = categoryResponse.Content.FirstOrDefault((x) => x.Id == p.CategoryId)!.Name;
                        Console.WriteLine($"Category Id: {p.CategoryId ?? "No selected category"}");
                        Console.WriteLine($"Category Name: {categoyName ?? "No selected category"}");
                    }

                    
                }
            }
            else
            {
                Console.WriteLine("No products found.");
            }
        }
        else
        {
            Console.WriteLine($"Something went wrong when fetching products.");
        }
    }


    // UPDATE PRODUCT
    internal void GetAndFindProductToUpdate()
    {
        Console.Clear();
        Response<IEnumerable<IProduct>> productResult = _productService.GetAllProducts();
        Response<IEnumerable<ICategory>> categoryResponse = _categoryService.GetAllCategories();

        if (productResult != null)
        {
            ListAllProducts();

            Console.WriteLine("Enter the product ID: ");
            string selectedId = (Console.ReadLine() ?? "").Trim();
            if (!string.IsNullOrEmpty(selectedId))
            {
                IEnumerable<IProduct> products = productResult.Content!;

                var productToUpdate = products.FirstOrDefault((x) => x.Id.ToLower() == selectedId.ToLower());
                if (productToUpdate != null)
                {
                    Console.Clear();
                    Console.WriteLine("---- PRODUCT TO UPDATE ----");
                    Console.WriteLine($"\nID: {productToUpdate.Id}");
                    Console.WriteLine($"Name: {productToUpdate.Name}");
                    Console.WriteLine($"Description: {productToUpdate.Description}");
                    Console.WriteLine($"Price: {productToUpdate.Price} kronor");

                    if (categoryResponse.Content != null && categoryResponse.Content.Any((x) => x.Id == productToUpdate.CategoryId))
                    {
                        var categoyName = categoryResponse.Content.FirstOrDefault((x) => x.Id == productToUpdate.CategoryId)!.Name;
                        Console.WriteLine($"Category Id: {productToUpdate.CategoryId ?? "No selected category"}");
                        Console.WriteLine($"Category Name: {categoyName ?? "No selected category"}");
                    }

                    Console.WriteLine("\n--------------------");

                    Product updatedProduct = new Product { Id = productToUpdate.Id };

                    Console.WriteLine("\nEnter new product name (required and unique): ");
                    updatedProduct.Name = (Console.ReadLine() ?? "").Trim();

                    Console.WriteLine("Enter new description (optional): ");
                    updatedProduct.Description = Console.ReadLine() ?? "";

                    Console.WriteLine("Enter the product's price (required, min 1):");
                    do
                    {
                        if (int.TryParse((Console.ReadLine() ?? "").Trim(), out int price) && price > 0)
                        {
                            updatedProduct.Price = price;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect price. Please try again.");
                        }
                    } while (updatedProduct.Price < 1);

                    // Om det finns några kategorier, begär cat-id och kolla om det finns. Då tilldela som cat-id. Om inga categories, skippa.
                    if (categoryResponse != null && categoryResponse.Content != null && categoryResponse.Content!.Any())
                    {
                        ListAllCategories();
                        Console.WriteLine("\nEnter new category ID (optional): ");
                        string categoryIdInput = "";
                        do
                        {
                            categoryIdInput = (Console.ReadLine() ?? "").Trim();
                            if (categoryResponse.Content.Any((x) => x.Id == categoryIdInput || string.IsNullOrEmpty(categoryIdInput)))
                            {
                                updatedProduct.CategoryId = categoryIdInput;

                            }
                            else if (!string.IsNullOrEmpty(categoryIdInput) && !categoryResponse.Content.Any((x) => x.Id == categoryIdInput))
                            {
                                Console.WriteLine("Incorrect category ID. Please try again.");

                            }
                        } while (updatedProduct.CategoryId != categoryIdInput);
                    }


                    Response<IProduct> requestResponse = _productService.UpdateProductById(selectedId, updatedProduct);
                    if (requestResponse != null)
                    {
                        // TODO göra till if else för succeeded eller ej senare kanske?? 
                        Console.WriteLine(requestResponse.Message);
                    }

                }
                else
                {
                    Console.WriteLine("Something whent wrong when fetching the product to update.");
                }
            }
            else
            {
                Console.WriteLine("You have to provide an ID.");
            }
        }
        else
        {
            Console.WriteLine("Something went wrong when fetching products.");
        }
    }

    // DELETE PRODUCT
    internal void GetIdToDeleteAndHandleDeleteProductRequest()
    {
        Console.Clear();
        ListAllProducts();

        Console.WriteLine("Enter the product ID: ");
        string selectedId = (Console.ReadLine() ?? "").Trim();

        if (!string.IsNullOrEmpty(selectedId))
        {
            Response<IProduct> requestResponse = _productService.DeleteProductById(selectedId);

            if (requestResponse != null)
            {
                // TODO göra till if else för succeeded eller ej senare kanske?? 
                Console.WriteLine(requestResponse.Message);
            }
        }
        else
        {
            Console.WriteLine("You have to provide an ID.");
        }
    }

    #endregion

}
