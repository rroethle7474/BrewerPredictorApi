using BrewerPredictorApi.Models.Dtos;
using BrewerPredictorApi.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewerPredictorApi.Services.Interfaces
{
    public interface IStandingsService
    {
        Task<bool> UpdateStandingsAsync(List<NLCentralStanding> standings);
        Task<List<StandingDto>> GetStandingsAsync();
    }
} 