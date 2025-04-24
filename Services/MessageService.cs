using BrewerPredictorApi.Data;
using BrewerPredictorApi.Models.Dtos;
using BrewerPredictorApi.Models.Entities;
using BrewerPredictorApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrewerPredictorApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;

        public MessageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MessageDto> AddMessageAsync(MessageRequestDto messageRequest)
        {
            try
            {
                var message = new Message
                {
                    Name = messageRequest.Name,
                    MessageText = messageRequest.Message,
                    HasResponded = messageRequest.HasResponded,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                };
                
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                
                return new MessageDto
                {
                    Id = message.Id,
                    Name = message.Name,
                    Message = message.MessageText,
                    HasResponded = message.HasResponded,
                    CreatedOn = message.CreatedOn,
                    UpdatedOn = message.UpdatedOn
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<MessageDto> EditMessageAsync(int id, MessageRequestDto messageRequest)
        {
            try
            {
                var message = await _context.Messages.FindAsync(id);
                
                if (message == null)
                    return null;
                    
                message.Name = messageRequest.Name;
                message.MessageText = messageRequest.Message;
                message.HasResponded = messageRequest.HasResponded;
                message.UpdatedOn = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                
                return new MessageDto
                {
                    Id = message.Id,
                    Name = message.Name,
                    Message = message.MessageText,
                    HasResponded = message.HasResponded,
                    CreatedOn = message.CreatedOn,
                    UpdatedOn = message.UpdatedOn
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<MessageDto>> GetMessagesAsync(int? id = null, string name = null)
        {
            try
            {
                IQueryable<Message> query = _context.Messages;
                
                if (id.HasValue)
                    query = query.Where(m => m.Id == id.Value);
                    
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(m => m.Name.Contains(name));
                    
                var messages = await query.ToListAsync();
                
                return messages.Select(m => new MessageDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Message = m.MessageText,
                    HasResponded = m.HasResponded,
                    CreatedOn = m.CreatedOn,
                    UpdatedOn = m.UpdatedOn
                }).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
} 