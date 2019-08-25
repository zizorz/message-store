using System.Collections.Generic;
using System.Threading.Tasks;
using MessageStoreApplication.Models;

namespace MessageStoreApplication.Storage
{
    public class MessageStore : IMessageStore
    {
        private readonly IDictionary<long, Message> _messages = new Dictionary<long, Message>();
        private long _idCounter = 0;
        
        public Task<long> AddAsync(Message message)
        {
            var id = GenerateId();
            message.Id = id;
            _messages.Add(id, message);
            return Task.FromResult(message.Id);
        }

        public Task<Message> GetAsync(long messageId)
        {
            if (_messages.ContainsKey(messageId))
            {
                return Task.FromResult(_messages[messageId]);
            }
            return Task.FromResult((Message)null);
        }

        public Task<ICollection<Message>> GetAllAsync()
        {
            return Task.FromResult(_messages.Values);
        }

        public Task<bool> UpdateAsync(Message message)
        {
            if (!_messages.ContainsKey(message.Id))
            {
                return Task.FromResult(false);
            }
            _messages[message.Id] = message;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(long messageId)
        {
            return Task.FromResult(_messages.Remove(messageId));
        }

        private long GenerateId()
        {
            return _idCounter++;
        }
    }
}