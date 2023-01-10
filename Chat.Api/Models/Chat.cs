namespace Chat.API.Models
{
    public class Chat
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public long ToUserId { get; set; }
        public virtual User ToUser { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
    }
}
