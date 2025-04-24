using BrewerPredictorApi.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewerPredictorApi.Services.Interfaces
{
    public interface IPredictionService
    {
        Task<PredictionDto> AddPredictionAsync(PredictionRequestDto prediction);
        Task<List<PredictionDto>> GetPredictionsAsync(int? id = null, string firstName = null, string lastName = null);
        Task<bool> DeletePredictionAsync(int id);
        Task<PredictionDto> EditPredictionAsync(int id, PredictionRequestDto prediction);
    }
} 