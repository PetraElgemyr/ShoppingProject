using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;
using ShoppingApp.Wpf.ViewModels.CategoryViewModels;
using System.Collections.ObjectModel;

namespace ShoppingApp.Wpf.ViewModels.ProductViewModels;

public partial class UpdateProductViewModel: ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly CurrentContextService _currentContextService;

    [ObservableProperty]
    private Product _currentProduct;

    [ObservableProperty]
    private ObservableCollection<Category> _categeories = [];


    [ObservableProperty]
    private string _messageAfterSave;


    //public UpdateProductViewModel(IServiceProvider serviceProvider, ProductService productService, CategoryService categoryService, CurrentContextService currentContextService)
    //{
    //    _serviceProvider = serviceProvider;
    //    _productService = productService;
    //    _categoryService = categoryService;
    //    _currentContextService = currentContextService;
    //    _currentProduct = _currentContextService.GetSelectedProduct();
    //    GetCategories();
    //    _messageAfterSave = "";

    //}

    public UpdateProductViewModel(IServiceProvider serviceProvider, ProductService productService, CategoryService categoryService, CurrentContextService currentContextService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _categoryService = categoryService;
        _currentContextService = currentContextService;
        _currentProduct = _currentContextService.GetSelectedProduct();
        _messageAfterSave = "";
        GetCategories();
    }


    [RelayCommand]
    public void GoToProductOverview()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductOverviewViewModel>();
    }

    [RelayCommand]
    public void GoToCategoryOverview()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CategoryOverviewViewModel>();
    }

    [RelayCommand]
    public void GetCategories()
    {
        var result = _categoryService.GetAllCategories();

        if (result.Content != null && result.Content.Any())
        {
            foreach (var c in result.Content)
            {
                Categeories.Add(c);
            }
        }
    }


    [RelayCommand]
    public void UpdateProduct(Product updatedProduct)
    {
        try
        {
            if (updatedProduct != null)
            {
                var result = _productService.UpdateProductById(updatedProduct.Id, updatedProduct);
                if (result.Succeeded == Status.Success)
                {
                    MessageAfterSave = "Product was successfully updated!";
                }
            }
        }
        catch (Exception)
        { }
    }
}
