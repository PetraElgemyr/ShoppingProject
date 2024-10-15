using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
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


    [ObservableProperty]
    private Product _product = new Product();

    [ObservableProperty]
    private ObservableCollection<Category> _categories = [];
    public CreateProductViewModel(IServiceProvider serviceProvider, ProductService productService, CategoryService categoryService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _categoryService = categoryService;
        GetCategories();

    }

    
    public void GetCategories()
    {
        try
        {
            var response = _categoryService.GetAllCategories();

            if (response.Content != null && response.Content.Any())
            {
                foreach (var category in response.Content)
                {
                    Categories.Add(category);
                }
            }
        }
        catch (Exception)
        {

        }

    }



    [RelayCommand]
    public void GoToAddNewCategory()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateCategoryViewModel>();
    }

    [RelayCommand]
    public void GoToProductOverview()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductOverviewViewModel>();
    }

    [RelayCommand]
    public void SaveProduct(Product product)
    {
        try
        {
            if (product != null)
            {
                _productService.CreateAndAddProductToList(product);
                Product = new Product();
            }
        }
        catch (Exception)
        {
        }

    }


}
