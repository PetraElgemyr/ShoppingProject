using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Services;

namespace ShoppingApp.Resources.Models;

public class ProductFileService : FileService, IProductFileService
{
    public ProductFileService(string filePath) : base(filePath)
    {
    }
}
