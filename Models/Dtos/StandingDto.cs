using System;

namespace BrewerPredictorApi.Models.Dtos
{
    public class StandingDto
    {
        public int Id { get; set; }
        public string Team { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TotalGames { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
} 