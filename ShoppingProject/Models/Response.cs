namespace ShoppingApp.Models
{
    public class Response
    {
        public bool Succeeded { get; set; }
        public object? Content { get; set; }
        public string? Message { get; set; }
    }
}
