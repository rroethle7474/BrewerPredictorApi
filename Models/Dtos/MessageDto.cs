using System;

namespace BrewerPredictorApi.Models.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public bool HasResponded { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
} 