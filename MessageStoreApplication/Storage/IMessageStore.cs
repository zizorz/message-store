using System.Collections.Generic;
using System.Threading.Tasks;
using MessageStoreApplication.Models;

namespace MessageStoreApplication.Storage
{
    public interface IMessageStore
    {
        Task<long> AddAsync(Message message);

        Task<Message> GetAsync(long messageId);

        Task<ICollection<Message>> GetAllAsync();

        Task<bool> UpdateAsync(Message message);

        Task<bool> RemoveAsync(long messageId);
    }
}