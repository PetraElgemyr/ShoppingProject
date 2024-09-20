using Resources.Models;

namespace Resources.Interfaces;

public interface IFileService
{
    public Response<string> SaveToFile(string content);

    public Response<string> GetFromFile();

}


