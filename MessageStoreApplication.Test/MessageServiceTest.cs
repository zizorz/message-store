using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MessageStoreApplication.Mapping;
using MessageStoreApplication.Models;
using MessageStoreApplication.Models.Exceptions;
using MessageStoreApplication.Services;
using MessageStoreApplication.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MessageStoreApplication.Test
{
    [TestClass]
    public class MessageServiceTest
    {

        private IMessageService _messageService;
        
        [TestInitialize]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new MappingProfile()));
            var mapper = mappingConfig.CreateMapper();
            _messageService = new MessageService(new MessageStore(), mapper);
        }
        
        [TestMethod]
        public async Task AddAsync_ShouldReturnIdOfAddedMessage()
        {
            var input = new MessageInput
            {
                Text = "Hello"
            };
            var result = await _messageService.AddAsync(input, "Client1");
            var result2 = await _messageService.AddAsync(input, "Client1");
            Assert.IsNotNull(result);
            Assert.IsNotNull(result2);
            Assert.AreNotEqual(result.MessageId, result2.MessageId);
        }
        
        [TestMethod]
        public async Task GetAsync_ShouldReturnAddedMessage()
        {
            var input = new MessageInput
            {
                Text = "Hello"
            };
            var input2 = new MessageInput
            {
                Text = "Bye"
            };
            await _messageService.AddAsync(input, "Client1");
            var id = (await _messageService.AddAsync(input2, "Client1")).MessageId;
            var result = await _messageService.GetAsync(id);
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(input2.Text, result.Text);
        }
        
        [TestMethod]
        public async Task GetAllAsync_ShouldReturnAllAddedMessages()
        {
            var input = new MessageInput
            {
                Text = "Hello"
            };
            var input2 = new MessageInput
            {
                Text = "Bye"
            };
            await _messageService.AddAsync(input, "Client1");
            await _messageService.AddAsync(input2, "Client1");
            var result = await _messageService.GetAllAsync();
            Assert.AreEqual(true, result.Any(x => x.Text == input.Text));
            Assert.AreEqual(true, result.Any(x => x.Text == input2.Text));
        }
        
        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateCorrectly()
        {
            var input = new MessageInput
            {
                Text = "Hello"
            };
            var input2 = new MessageInput
            {
                Text = "Bye"
            };
            var id = (await _messageService.AddAsync(input, "Client1")).MessageId;
            await _messageService.AddAsync(input2, "Client1");
            
            var input3 = new MessageInput
            {
                Text = "UpdatedText"
            };
            await _messageService.UpdateAsync(id, input3, "Client1");
            var result = await _messageService.GetAsync(id);
            Assert.AreEqual(input3.Text, result.Text);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ForbiddenException))]
        public async Task UpdateAsync_ShouldThrowForbiddenExceptionWhenOwnerIdDoesNotMatch()
        {
            var input = new MessageInput
            {
                Text = "Hello"
            };
            var id = (await _messageService.AddAsync(input, "Client1")).MessageId;
            
            var input2 = new MessageInput
            {
                Text = "UpdatedText"
            };
            await _messageService.UpdateAsync(id, input2, "Client2");
        }
        
        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task DeleteAsync_ShouldDeleteCorrectly()
        {
            var input = new MessageInput
            {
                Text = "Hello"
            };
            var input2 = new MessageInput
            {
                Text = "Bye"
            };
            var id = (await _messageService.AddAsync(input, "Client1")).MessageId;
            await _messageService.AddAsync(input2, "Client1");
            
            await _messageService.DeleteAsync(id, "Client1");
            await _messageService.GetAsync(id);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ForbiddenException))]
        public async Task DeleteAsync_ShouldThrowForbiddenExceptionWhenOwnerIdDoesNotMatch()
        {
            var input = new MessageInput
            {
                Text = "Hello"
            };
            var id = (await _messageService.AddAsync(input, "Client1")).MessageId;
            await _messageService.DeleteAsync(id, "Client2");
        }
    }
}