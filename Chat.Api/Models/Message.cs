namespace Fullstack.API.Models
{
    public class Message
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public long ToUserId { get; set; }
        public virtual User ToUser { get; set; }
        public bool Sent { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public string Reply { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
