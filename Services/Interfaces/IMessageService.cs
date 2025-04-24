using BrewerPredictorApi.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewerPredictorApi.Services.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDto> AddMessageAsync(MessageRequestDto message);
        Task<MessageDto> EditMessageAsync(int id, MessageRequestDto message);
        Task<List<MessageDto>> GetMessagesAsync(int? id = null, string name = null);
    }
} 