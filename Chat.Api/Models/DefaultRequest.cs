namespace Chat.API.Models
{
    public class DefaultResult<T>
    {
        public DefaultResult()
        {
            Success = false;
            Message = "Error";
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
