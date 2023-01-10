namespace Chat.API.Models
{
    public class RefreshToken
    {
        public long Id { get; set; }
        public string Token { get; set; }
        public long UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
