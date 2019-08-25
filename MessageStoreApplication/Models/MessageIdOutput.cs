namespace MessageStoreApplication.Models
{
    public class MessageIdOutput
    {
        public MessageIdOutput(long id)
        {
            MessageId = id;
        }
        
        public long MessageId { get; set; }
    }
}