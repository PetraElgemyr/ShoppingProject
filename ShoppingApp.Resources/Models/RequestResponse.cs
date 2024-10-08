using ShoppingApp.Resources.Enums;

namespace ShoppingApp.Resources.Models;

public class RequestResponse<T> where T : class
{

        public Status Succeeded { get; set; }
        public T? Content { get; set; }
        public string? Message { get; set; }
    

}
