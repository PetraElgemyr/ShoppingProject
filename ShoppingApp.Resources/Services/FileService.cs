using ShoppingApp.Resources.Enums;
using ShoppingApp.Resources.Interfaces;
using ShoppingApp.Resources.Models;

namespace ShoppingApp.Resources.Services;

public class FileService(string filePath) : IFileService
{
    private readonly string _filePath = filePath;

    public RequestResponse<string> GetFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("File not found.");
            }
            using var sr = new StreamReader(_filePath);
            var content = sr.ReadToEnd();

            return new RequestResponse<string> { Succeeded = Status.Success, Content = content };
        }
        catch (Exception ex)
        {
            return new RequestResponse<string> { Succeeded = Status.Failed, Message = $"{ex.Message}" };
        }
    }
    public RequestResponse<string> SaveToFile(string content)
    {
        try
        {
            //Directory.CreateDirectory() // Skapa katalog och stoppa filen i den katalogen om man vill
            if (!File.Exists(_filePath))
            {
                return new RequestResponse<string> { Succeeded = Status.Failed, Message = "File not found." };
            }

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
