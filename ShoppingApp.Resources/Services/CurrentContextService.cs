using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Services;

public class CurrentContextService
{

    private Product _product = new();
    private Category _category = new();
    private string _categoryId = "";

    public void SetSelectedCategoryId(string categoryId)
    {
        _categoryId = categoryId;
    }

    public string GetSelectedCategoryId()
    {
        return _categoryId;
    }

    public void SetSelectedProduct (Product product)
    {
        _product = product;
    }

    public Product GetSelectedProduct()
    {
        return _product;
    }

    public void SetSelectedCategory(Category category)
    {
        _category = category;
    }

    public Category GetSelectedCategory()
    {
        return _category;
    }
}
