using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MessageStoreApplication.Models;
using MessageStoreApplication.Models.Exceptions;
using MessageStoreApplication.Storage;

namespace MessageStoreApplication.Services
{
    public class MessageService : IMessageService
    {

        private readonly IMessageStore _messageStore;
        private readonly IMapper _mapper;
        
        public MessageService(IMessageStore messageStore, IMapper mapper)
        {
            _messageStore = messageStore;
            _mapper = mapper;
        }

        public async Task<MessageIdOutput> AddAsync(MessageInput messageInput, string ownerId)
        {
            var message = _mapper.Map<MessageInput, Message>(messageInput);
            message.OwnerId = ownerId;
            var messageId = await _messageStore.AddAsync(message);
            return new MessageIdOutput(messageId);
        }

        public async Task<MessageOutput> GetAsync(long messageId)
        {
            var message = await _messageStore.GetAsync(messageId);
            if (message == null)
            {
                throw new NotFoundException();
            }
            return _mapper.Map<Message, MessageOutput>(message);
        }

        public async Task<ICollection<MessageOutput>> GetAllAsync()
        {
            var messages = await _messageStore.GetAllAsync();
            return _mapper.Map<ICollection<Message>, ICollection<MessageOutput>>(messages);
        }

        public async Task UpdateAsync(long messageId, MessageInput messageInput, string ownerId)
        {
            await ValidateOwnerAsync(messageId, ownerId);
            var message = new Message
            {
                Id = messageId,
                OwnerId = ownerId,
                Text = messageInput.Text
            };
            await _messageStore.UpdateAsync(message);
        }

        public async Task DeleteAsync(long messageId, string ownerId)
        {
            await ValidateOwnerAsync(messageId, ownerId);
            await _messageStore.RemoveAsync(messageId);
        }

        private async Task ValidateOwnerAsync(long messageId, string ownerId)
        {
            var message = await _messageStore.GetAsync(messageId);
            if (message == null)
            {
                throw new NotFoundException();
            }
            if (message.OwnerId != ownerId)
            {
                throw new ForbiddenException();
            }
        }
    }
}