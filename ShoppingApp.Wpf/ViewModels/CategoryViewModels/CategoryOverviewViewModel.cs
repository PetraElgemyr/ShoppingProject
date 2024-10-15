using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;
using ShoppingApp.Wpf.ViewModels.ProductViewModels;
using System.Collections.ObjectModel;

namespace ShoppingApp.Wpf.ViewModels.CategoryViewModels;

public partial class CategoryOverviewViewModel : ObservableObject
{

    private readonly IServiceProvider _serviceProvider;
    private readonly CategoryService _categoryService;


    [ObservableProperty]
    private ObservableCollection<Category> _categories = [];

    [ObservableProperty]
    private Category _category = new();


    public CategoryOverviewViewModel(IServiceProvider serviceProvider, CategoryService categoryService)
    {
        _serviceProvider = serviceProvider;
        _categoryService = categoryService;
        GetCategories();
    }


    [RelayCommand]
    public void GoToProductOverview()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductOverviewViewModel>();
    }

    [RelayCommand]
    public void GoToCreateCategory()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CreateCategoryViewModel>();
    }

    [RelayCommand]
    public void GoToUpdateCategory()
    {
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
    public void UpdateCategory()
    {

    }

    [RelayCommand]
    public void DeleteCategory()
    {

    }
}
