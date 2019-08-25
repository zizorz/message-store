namespace MessageStoreApplication.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public string OwnerId { get; set; }
    }
}