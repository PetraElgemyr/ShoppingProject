using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;
using ShoppingApp.Wpf.ViewModels.ProductViewModels;
using System.Collections.ObjectModel;

namespace ShoppingApp.Wpf.ViewModels.CategoryViewModels;

public partial class CategoryOverviewViewModel : ObservableObject
{

    private readonly IServiceProvider _serviceProvider;
    private readonly CategoryService _categoryService;
    private readonly CurrentContextService _currentContextService;
    private readonly ProductService _productService;


    [ObservableProperty]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty]
    private Category _category = new();


    public CategoryOverviewViewModel(IServiceProvider serviceProvider, CategoryService categoryService, ProductService productService, CurrentContextService currentContextService)
    {
        _serviceProvider = serviceProvider;
        _categoryService = categoryService;
        _productService = productService;
        _currentContextService = currentContextService;
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
    public void GoToCreateCategory()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateCategoryViewModel>();
    }


    [RelayCommand]
    public void GoToAddProduct()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateProductViewModel>();
    }


    [RelayCommand]
    public void GoToUpdateCategory(Category category)
    {
        _currentContextService.SetSelectedCategory(category);
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UpdateCategoryViewModel>();
    }

    public void GetCategories()
    {
        try
        {
            var fetchedCategories = _categoryService.GetAllCategories().Content;
            if (fetchedCategories != null)
            {
                Categories.Clear();

                foreach (var c in fetchedCategories)
                {
                    Categories.Add(c);
                }
            }
        }
        catch (Exception)
        {
        }

    }

    [RelayCommand]
    public void SaveCategory(Category category)
    {
        try
        {
            if (category != null)
            {
                _categoryService.CreateAndAddCategoryToList(category);
                GetCategories();
            }
        }
        catch (Exception)
        {
        }
    }

    [RelayCommand]
    public void UpdateCategory(Category category)
    {
        try
        {

        }
        catch (Exception)
        { }
    }

    [RelayCommand]
    public void DeleteCategory(Category category)
    {

        try
        {
          var categoryRequestResult =  _categoryService.DeleteCategoryById(category.Id);
            if (categoryRequestResult.Succeeded == Status.Success)
            {
                _productService.DeleteProductsWithSpecificCategoryId(category.Id);
                GetCategories();
            }
        }
        catch (Exception)
        { }
    }
}
