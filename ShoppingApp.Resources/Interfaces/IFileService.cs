using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Interfaces;

public interface IFileService
{
    public RequestResponse<string> GetFromFile();
    public RequestResponse<string> SaveToFile(string content);
}
