using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Services;

public class CurrentContextService
{

    private Product _product = new();
    private Category _category = new();

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
