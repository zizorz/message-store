using System.Collections.Generic;
using System.Threading.Tasks;
using MessageStoreApplication.Models;

namespace MessageStoreApplication.Services
{
    public interface IMessageService
    {
        Task<MessageIdOutput> AddAsync(MessageInput messageInput, string ownerId);

        Task<MessageOutput> GetAsync(long messageId);

        Task<ICollection<MessageOutput>> GetAllAsync();

        Task UpdateAsync(long messageId, MessageInput message, string ownerId);

        Task DeleteAsync(long messageId, string ownerId);
    }
}