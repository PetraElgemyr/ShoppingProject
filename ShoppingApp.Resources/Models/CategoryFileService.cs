using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Services;

namespace ShoppingApp.Resources.Models;

public class CategoryFileService : FileService, ICategoryFileService
{
    public CategoryFileService(string filePath) : base(filePath)
    {
    }
}
