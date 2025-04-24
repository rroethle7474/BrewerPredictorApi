using System.ComponentModel.DataAnnotations;

namespace BrewerPredictorApi.Models.Dtos
{
    public class MessageRequestDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [Required]
        public string Message { get; set; }
        
        public bool HasResponded { get; set; }
    }
} 