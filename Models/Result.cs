namespace Shoezy.Models
{
    public class Result<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }

        public string Error { get; set; }
    }
}
