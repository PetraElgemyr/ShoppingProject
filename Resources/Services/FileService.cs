using Resources.Interfaces;
using Resources.Models;

namespace Resources.Services;

public class FileService(string filePath) : IFileService
{
    private readonly string _filePath = filePath;

    public Response<string> GetFromFile()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException("File not found.");
            }
            using var sr = new StreamReader(_filePath);
            var content = sr.ReadToEnd();

            return new Response<string> { Succeeded = true, Content = content };
        }
        catch (Exception ex)
        {
            return new Response<string> { Succeeded = false, Message = $"{ex.Message}" };
        }
    }
    public Response<string> SaveToFile(string content)
    {
        try
        {
            //Directory.CreateDirectory() // Skapa katalog och stoppa filen i den katalogen om man vill
            using var sw = new StreamWriter(_filePath, false);
            sw.WriteLine(content);

            return new Response<string> { Succeeded = true };
        }
        catch (Exception ex)
        {
            return new Response<string> { Succeeded = false, Message = $"{ex.Message}" };
        }
    }
}
