using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;
using ShoppingApp.Wpf.ViewModels.CategoryViewModels;
using System.Collections.ObjectModel;

namespace ShoppingApp.Wpf.ViewModels.ProductViewModels;

public partial class ProductOverviewViewModel : ObservableObject
{

    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;
    private readonly CurrentContextService _currentContextService;

    [ObservableProperty]
    private ObservableCollection<Product> _products = [];

    [ObservableProperty]
    private Product _product = new Product();

    public ProductOverviewViewModel(IServiceProvider serviceProvider, ProductService productService, CurrentContextService currentContextService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _currentContextService = currentContextService;
        GetProducts();
    }

    [RelayCommand]
    public void GoToCategoryOverview()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CategoryOverviewViewModel>();
    }


    [RelayCommand]
    public void GoToAddProduct()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateProductViewModel>();
    }

    [RelayCommand]
    public void GoToUpdateProduct(Product productToUpdate)
    {
        _currentContextService.SetSelectedProduct(productToUpdate);
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UpdateProductViewModel>();
    }

 




    [RelayCommand]
    public void DeleteProduct(Product productToDelete)
    {
        try
        {
            if (productToDelete != null)
            {
                var result = _productService.DeleteProductById(productToDelete.Id);
                if (result.Succeeded == Status.Success)
                {
                    GetProducts();
                }
            }
        }
        catch (Exception) { }
    }

    public void GetProducts()
    {
        try
        {
            Products.Clear();
            var fetchedProducts = _productService.GetAllProducts().Content;

            if (fetchedProducts != null && fetchedProducts.Any())
            {
                foreach (var p in fetchedProducts)
                {
                    Products.Add(p);
                }
            }
        }
        catch (Exception)
        {
        }
    }


}
