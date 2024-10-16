using Microsoft.Extensions.DependencyInjection;
using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Models;
using ShoppingApp.Resources.Services;
using ShoppingApp.Wpf.ViewModels.CategoryViewModels;
using ShoppingApp.Wpf.ViewModels.ProductViewModels;
using ShoppingApp.Wpf.Views.CategoryViews;
using ShoppingApp.Wpf.Views.ProductViews;
using System.IO;
using System.Windows;

namespace ShoppingApp.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    public static IServiceProvider? ServiceProvider { get; private set; }

    private void ConfigureServices(IServiceCollection services)
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string categoryFilePath = Path.Combine(baseDirectory, "categories.json");
        string productFilePath = Path.Combine(baseDirectory, "products.json");

        // register file services med olika filepaths
        services.AddSingleton<ICategoryFileService>(sp => new CategoryFileService(categoryFilePath));
        services.AddSingleton<IProductFileService>(sp => new ProductFileService(productFilePath));

        // Register services
        services.AddSingleton<ProductService>();
        services.AddSingleton<CategoryService>();
        services.AddSingleton<CurrentContextService>();

        // Register ViewModels
        services.AddSingleton<MainWindowViewModel>();

        // product viewmodels
        services.AddTransient<ProductOverviewViewModel>();
        services.AddTransient<CreateProductViewModel>();
        services.AddTransient<UpdateProductViewModel>();
        // category viewmodels
        services.AddTransient<CategoryOverviewViewModel>();
        services.AddTransient<CreateCategoryViewModel>();
        services.AddTransient<UpdateCategoryViewModel>();

        // Register Views
        services.AddSingleton<MainWindow>();

        // product views
        services.AddTransient<ProductOverviewView>();
        services.AddTransient<CreateProductView>();
        services.AddTransient<UpdateProductView>();
        // category views
        services.AddTransient<CategoryOverviewView>();
        services.AddTransient<CreateCategoryView>();
        services.AddTransient<UpdateCategoryView>();
    }


    protected override void OnStartup(StartupEventArgs e)
    {
        var serviceColletion = new ServiceCollection();
        ConfigureServices(serviceColletion);

        ServiceProvider = serviceColletion.BuildServiceProvider();
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

        // Istället för att ha i MainWindowViewModel.xaml.cs
        mainWindow.DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>(); 
        mainWindow.Show();

        base.OnStartup(e);
    }

}
