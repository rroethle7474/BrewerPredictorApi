using BrewerPredictorApi.Models.Dtos;
using BrewerPredictorApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BrewerPredictorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(IMessageService messageService, ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody] MessageRequestDto messageRequest)
        {
            try
            {
                var message = await _messageService.AddMessageAsync(messageRequest);
                return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding message");
                return StatusCode(500, "An error occurred while adding message");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages([FromQuery] int? id = null, [FromQuery] string name = null)
        {
            try
            {
                var messages = await _messageService.GetMessagesAsync(id, name);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving messages");
                return StatusCode(500, "An error occurred while retrieving messages");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            try
            {
                var messages = await _messageService.GetMessagesAsync(id);
                var message = messages.FirstOrDefault();
                
                if (message == null)
                    return NotFound();
                    
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving message with ID {id}");
                return StatusCode(500, "An error occurred while retrieving message");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditMessage(int id, [FromBody] MessageRequestDto messageRequest)
        {
            try
            {
                var message = await _messageService.EditMessageAsync(id, messageRequest);
                
                if (message == null)
                    return NotFound();
                    
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error editing message with ID {id}");
                return StatusCode(500, "An error occurred while editing message");
            }
        }
    }
}
