using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;
using ShoppingApp.Wpf.ViewModels.ProductViewModels;

namespace ShoppingApp.Wpf.ViewModels.CategoryViewModels;

public partial class UpdateCategoryViewModel : ObservableObject
{

    private readonly IServiceProvider _serviceProvider;
    private readonly CategoryService _categoryService;
    private readonly CurrentContextService _currentContextService;
    

    [ObservableProperty]
    private Category _currentCategory;
        
    [ObservableProperty]
    private string _messageAfterUpdate;

    public UpdateCategoryViewModel(IServiceProvider serviceProvider, CategoryService categoryService, CurrentContextService currentContextService)
    {
        _serviceProvider = serviceProvider;
        _categoryService = categoryService;
        _currentContextService = currentContextService;
        _currentCategory = _currentContextService.GetSelectedCategory();
        _messageAfterUpdate = "";
    }

    [RelayCommand]
    public void GoToCategoryOverview()
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainWindow.CurrentViewModel = _serviceProvider.GetRequiredService<CategoryOverviewViewModel>();
    }

    [RelayCommand]
    public void GoToProductOverview()
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainWindow.CurrentViewModel = _serviceProvider.GetRequiredService<ProductOverviewViewModel>();
    }


    [RelayCommand]
    public void SaveUpdatedCategory(Category category)
    {
        try
        {
            if (category != null)
            {
               var result = _categoryService.UpdateCategoryById(category.Id, category);
                
                if(result.Succeeded == Status.Success && !string.IsNullOrEmpty(result.Message))
                {
                    MessageAfterUpdate = result.Message;
                    _currentContextService.SetSelectedCategory(new Category());                }
                else
                {
                    MessageAfterUpdate = result.Message ?? "";
                }
            }
        }
        catch (Exception ex)
        {
            MessageAfterUpdate = ex.Message;
        }
    }


}
