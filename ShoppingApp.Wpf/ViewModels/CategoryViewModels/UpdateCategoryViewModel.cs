using CommunityToolkit.Mvvm.ComponentModel;
using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;

namespace ShoppingApp.Wpf.ViewModels.CategoryViewModels;

public partial class UpdateCategoryViewModel : ObservableObject
{

    private readonly IServiceProvider _serviceProvider;
    private readonly CategoryService _categoryService;
    private readonly CurrentContextService _currentContextService;

    [ObservableProperty]
    private Category _currentCategory;
        
    [ObservableProperty]
    private string _messageAfterSave;

    public UpdateCategoryViewModel(IServiceProvider serviceProvider, CategoryService categoryService, CurrentContextService currentContextService)
    {
        _serviceProvider = serviceProvider;
        _categoryService = categoryService;
        _currentContextService = currentContextService;
        _currentCategory = _currentContextService.GetSelectedCategory();
        _messageAfterSave = "";
    }



}
