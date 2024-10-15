using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;
using ShoppingApp.Wpf.ViewModels.ProductViewModels;
using System.Collections.ObjectModel;

namespace ShoppingApp.Wpf.ViewModels.CategoryViewModels;

public partial class CreateCategoryViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly CurrentContextService _currentContextService;


    [ObservableProperty]
    private Category _category = new();

    [ObservableProperty]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty]
    private string _messageAfterSave = "";

    public CreateCategoryViewModel(IServiceProvider serviceProvider, CategoryService categoryService, ProductService productService, CurrentContextService currentContextService)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _categoryService = categoryService;
        _currentContextService = currentContextService;
    }


    [RelayCommand]
    public void GoToCategoryOverview()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CategoryOverviewViewModel>();
    }

    [RelayCommand]
    public void GoToProductOverview()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductOverviewViewModel>();
    }


    [RelayCommand]
    public void SaveCategory(Category category)
    {
        try
        {
            var response = _categoryService.CreateAndAddCategoryToList(category);

            if (response.Succeeded == Status.Success)
            {
                MessageAfterSave = "Category was successfully saved";
            }
            else if (response.Succeeded == Status.SuccessWithErrors && !string.IsNullOrEmpty(response.Message))
            {
                MessageAfterSave = response.Message;
            } else
            {
                MessageAfterSave = "Oops! Something went wrong.";
            }
        }
        catch (Exception)
        {}

    }
}
