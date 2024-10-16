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
    private ObservableCollection<Category> _categories = [];


    [ObservableProperty]
    private string _messageAfterSave;

    [ObservableProperty]
    private string _errorMessage;


    public UpdateProductViewModel(IServiceProvider serviceProvider, ProductService productService, CategoryService categoryService, CurrentContextService currentContextService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _categoryService = categoryService;
        _currentContextService = currentContextService;
        _currentProduct = _currentContextService.GetSelectedProduct();
        _messageAfterSave = "";
        _errorMessage = "";
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
                Categories.Add(c);
            }
        }
    }



    // TODO. skapa klickevent på categorierna, som tillsätter currentProduct.CategoryId
    [RelayCommand]
    public void UpdateCategoryIdForCurrentProduct(string id)
    {
        try
        {
            if (!string.IsNullOrEmpty(id))
            {
                CurrentProduct.CategoryId = id;
                _currentContextService.SetSelectedProduct(CurrentProduct);

                // sätt om, dvs "uppdatera" viewmodelen för att visa uppdaterade categoryId
                var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
                mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UpdateProductViewModel>();

            } else
            {
                ErrorMessage = "No category ID was assigned to product";
            }
        }
        catch (Exception)
        {}
    }

    [RelayCommand]
    public void UpdateProduct(Product updatedProduct)
    {
        try
        {
            if (updatedProduct != null)
            {
                var result = _productService.UpdateProductById(updatedProduct.Id, updatedProduct);
                if (result.Succeeded == Status.Success && !string.IsNullOrEmpty(result.Message))
                {
                    MessageAfterSave = result.Message;
                    // nollställ produkten i contextet, men ej lokala currentProduct
                    _currentContextService.SetSelectedProduct(new Product());
                }
            }
        }
        catch (Exception ex)
        {
            MessageAfterSave = ex.Message;
        }
    }
}
