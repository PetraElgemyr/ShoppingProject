using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;
using ShoppingApp.Wpf.ViewModels.CategoryViewModels;
using System.Collections.ObjectModel;

namespace ShoppingApp.Wpf.ViewModels.ProductViewModels;

public partial class CreateProductViewModel : ObservableObject
{

    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;
    readonly CurrentContextService _currentContextService;


    [ObservableProperty]
    private Product _product = new();


    [ObservableProperty]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty]
    private string _messageAfterSave = "";

    public CreateProductViewModel(IServiceProvider serviceProvider, ProductService productService, CategoryService categoryService, CurrentContextService currentContextService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _categoryService = categoryService;
        _currentContextService = currentContextService;

        if (_productService.GetOneProductById(_product.Id).Succeeded != Status.Success)
        {
            // om produkten ej är skapad i filen ännu, hämta valt categoriID från context
            var categoryIdFromContext = _currentContextService.GetSelectedCategoryId();
            if (!string.IsNullOrEmpty(categoryIdFromContext))
            {
                _product = _currentContextService.GetSelectedProduct();
            }
            else
            {
                _product.CategoryId = "";
            }
        }
        GetCategories();

    }


    [RelayCommand]
    public void GetCategories()
    {
        var result = _categoryService.GetAllCategories();

        if (result.Content != null && result.Content.Any())
        {
            foreach (var c in result.Content)
            {
                Categories.Add(c);
            }
        }
    }




    [RelayCommand]
    public void GoToCategoryOverview()
    {
        // vid varje navigering till annan sida, RENSA!
        ClearSelectedObjectsInContext();

        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CategoryOverviewViewModel>();
    }

    [RelayCommand]
    public void GoToProductOverview()
    {
        ClearSelectedObjectsInContext();
         var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductOverviewViewModel>();
    }


    [RelayCommand]
    public void GoToCreateCategory()
    {
        _currentContextService.SetSelectedCategory(new Category());
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateCategoryViewModel>();
    }


    [RelayCommand]
    public void GoToAddProduct()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateProductViewModel>();
    }


    public void ClearSelectedObjectsInContext()
    {
        _currentContextService.SetSelectedProduct(new Product());
        _currentContextService.SetSelectedCategoryId("");
    }


    [RelayCommand]
    public void UpdateCategoryIdForCurrentProduct(string id)
    {
        try
        {
            if (!string.IsNullOrEmpty(id))
            {


                Product.CategoryId = id;
                _currentContextService.SetSelectedCategoryId(id);
                _currentContextService.SetSelectedProduct(Product);

                // sätt om, dvs "uppdatera" viewmodelen för att visa uppdaterade categoryId
                var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                    mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateProductViewModel>();
            
            }
        }
        catch (Exception)
        { }
    }



    [RelayCommand]
    public void SaveProduct()
    {
        try
        {
            if (Product != null)
            {

                if (!Categories.Any(x => x.Id == Product.CategoryId))
                {
                    MessageAfterSave = "Category ID does not exist.";
                    return;
                }


                if (!decimal.TryParse(Product.Price.ToString(), out decimal price))
                {
                    MessageAfterSave = "Price must be a number.";
                    return;
                }

                var result = _productService.CreateAndAddProductToList(Product);

                if (result.Succeeded == Status.Success)
                {
                    // måste tömma och nollställa min produkt samt valt category id
                    Product = new Product();
                    _currentContextService.SetSelectedProduct(Product);
                    _currentContextService.SetSelectedCategoryId("");
                }
                MessageAfterSave = result.Message ?? "";
            }
        }
        catch (Exception)
        { }
    }
}
