using System;

namespace BrewerPredictorApi.Models.Entities
{
    public class NLCentralStanding
    {
        public int Id { get; set; }
        public string Team { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int TotalGames { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
} 