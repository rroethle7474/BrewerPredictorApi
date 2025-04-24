using System.ComponentModel.DataAnnotations;

namespace BrewerPredictorApi.Models.Dtos
{
    public class PredictionRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [MaxLength(100)]
        public string LastName { get; set; }
        
        [Required]
        [Range(0, 162)]
        public int Wins { get; set; }
    }
} 