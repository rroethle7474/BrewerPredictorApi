using System;

namespace BrewerPredictorApi.Models.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MessageText { get; set; }
        public bool HasResponded { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
} 