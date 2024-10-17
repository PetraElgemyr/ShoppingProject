using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Services;

public abstract class FileService : IFileService
{
    private readonly string _filePath;
    protected FileService(string filePath)
    {
        _filePath = filePath;
    }


    public RequestResponse<string> GetFromFile()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                using var sr = new StreamReader(_filePath);
                var content = sr.ReadToEnd();

                if(content != null)
                {
                return new RequestResponse<string> { Succeeded = Status.Success, Content = content };

                }
            }
            else
            {
                return new RequestResponse<string> { Succeeded = Status.NotFound, Message = "File not found"};

            }
         

        }
        catch (Exception ex)
        {
            return new RequestResponse<string> { Succeeded = Status.Failed, Message = $"{ex.Message}" };
        }
        return default!;
    }
    public RequestResponse<string> SaveToFile(string content)
    {
        try
        {
            //Directory.CreateDirectory() // Skapa katalog och stoppa filen i den katalogen om man vill
          

            using var sw = new StreamWriter(_filePath, false);
            sw.WriteLine(content);
            return new RequestResponse<string> { Succeeded = Status.Success };
        }
        catch (Exception ex)
        {
            return new RequestResponse<string> { Succeeded = Status.Failed, Message = $"{ex.Message}" };
        }
    }
}
