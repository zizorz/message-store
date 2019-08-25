using System.ComponentModel.DataAnnotations;

namespace MessageStoreApplication.Models
{
    public class MessageInput
    {
        [Required]
        public string Text { get; set; }
    }
}