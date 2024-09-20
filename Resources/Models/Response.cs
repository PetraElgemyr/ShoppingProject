namespace Resources.Models;

public class Response<T> where T : class
{
    public bool Succeeded { get; set; }
    public T? Content { get; set; }
    public string? Message { get; set; }
}


