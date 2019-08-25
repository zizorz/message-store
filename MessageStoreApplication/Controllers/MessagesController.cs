using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MessageStoreApplication.Models;
using MessageStoreApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageStoreApplication.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        
        // GET api/messages
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<MessageOutput>>> Get()
        {
            var messages = await _messageService.GetAllAsync();
            return Ok(messages);
        }

        // GET api/messages/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<string>> Get(long id)
        {
            var message = await _messageService.GetAsync(id);
            return Ok(message);
        }

        // POST api/messages
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<MessageIdOutput>> Post([FromBody] MessageInput messageInput)
        {
            var messageIdOutput = await _messageService.AddAsync(messageInput, GetUserId());
            return Ok(messageIdOutput);
        }

        // PUT api/messages/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put(long id, [FromBody] MessageInput messageInput)
        {
            await _messageService.UpdateAsync(id, messageInput, GetUserId());
            return Ok();
        }

        // DELETE api/messages/5
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(long id)
        {
            await _messageService.DeleteAsync(id, GetUserId());
            return Ok();
        }

        private string GetUserId()
        {
            var authHeader = Request.Headers["Authorization"];
            return authHeader;
        }
    }
}