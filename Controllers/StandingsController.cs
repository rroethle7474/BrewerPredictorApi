using BrewerPredictorApi.Models.Dtos;
using BrewerPredictorApi.Models.Entities;
using BrewerPredictorApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BrewerPredictorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StandingsController : ControllerBase
    {
        private readonly IStandingsService _standingsService;
        private readonly ILogger<StandingsController> _logger;

        public StandingsController(IStandingsService standingsService, ILogger<StandingsController> logger)
        {
            _standingsService = standingsService;
            _logger = logger;
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStandings([FromBody] List<StandingDto> standings)
        {
            try
            {
                var mappedStandings = standings.Select(s => new NLCentralStanding
                {
                    Id = s.Id,
                    Team = s.Team,
                    Wins = s.Wins,
                    Losses = s.Losses,
                    TotalGames = s.TotalGames,
                    UpdatedOn = DateTime.UtcNow
                }).ToList();

                var result = await _standingsService.UpdateStandingsAsync(mappedStandings);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating standings");
                return StatusCode(500, "An error occurred while updating standings");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStandings()
        {
            try
            {
                var standings = await _standingsService.GetStandingsAsync();
                return Ok(standings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving standings");
                return StatusCode(500, "An error occurred while retrieving standings");
            }
        }
    }
}
