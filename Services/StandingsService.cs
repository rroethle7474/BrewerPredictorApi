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
    public class StandingsService : IStandingsService
    {
        private readonly ApplicationDbContext _context;

        public StandingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateStandingsAsync(List<NLCentralStanding> standings)
        {
            try
            {
                // Get existing standings
                var existingStandings = await _context.NLCentralStandings.ToListAsync();
                
                foreach (var standing in standings)
                {
                    var existingStanding = existingStandings.FirstOrDefault(s => s.Id == standing.Id);
                    
                    if (existingStanding != null)
                    {
                        // Update existing
                        existingStanding.Team = standing.Team;
                        existingStanding.Wins = standing.Wins;
                        existingStanding.Losses = standing.Losses;
                        existingStanding.TotalGames = standing.TotalGames;
                        existingStanding.UpdatedOn = DateTime.UtcNow;
                    }
                    else
                    {
                        // Add new
                        standing.CreatedOn = DateTime.UtcNow;
                        standing.UpdatedOn = DateTime.UtcNow;
                        _context.NLCentralStandings.Add(standing);
                    }
                }
                
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<StandingDto>> GetStandingsAsync()
        {
            try
            {
                var standings = await _context.NLCentralStandings
                    .OrderByDescending(s => s.Wins)
                    .ThenBy(s => s.Losses)
                    .ToListAsync();
                    
                return standings.Select(s => new StandingDto
                {
                    Id = s.Id,
                    Team = s.Team,
                    Wins = s.Wins,
                    Losses = s.Losses,
                    TotalGames = s.TotalGames,
                    UpdatedOn = s.UpdatedOn
                }).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
} 