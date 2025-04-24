using BrewerPredictorApi.Models.Dtos;
using BrewerPredictorApi.Services.Exceptions;
using BrewerPredictorApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BrewerPredictorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionsController : ControllerBase
    {
        private readonly IPredictionService _predictionService;
        private readonly ILogger<PredictionsController> _logger;

        public PredictionsController(IPredictionService predictionService, ILogger<PredictionsController> logger)
        {
            _predictionService = predictionService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrediction([FromBody] PredictionRequestDto predictionRequest)
        {
            try
            {
                var prediction = await _predictionService.AddPredictionAsync(predictionRequest);
                return CreatedAtAction(nameof(GetPrediction), new { id = prediction.Id }, prediction);
            }
            catch (DuplicatePredictionException ex)
            {
                _logger.LogWarning(ex, "Duplicate prediction attempt");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding prediction");
                return StatusCode(500, "An error occurred while adding prediction");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPredictions([FromQuery] int? id = null, [FromQuery] string firstName = null, [FromQuery] string lastName = null)
        {
            try
            {
                var predictions = await _predictionService.GetPredictionsAsync(id, firstName, lastName);
                return Ok(predictions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving predictions");
                return StatusCode(500, "An error occurred while retrieving predictions");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPrediction(int id)
        {
            try
            {
                var predictions = await _predictionService.GetPredictionsAsync(id);
                var prediction = predictions.FirstOrDefault();
                
                if (prediction == null)
                    return NotFound();
                    
                return Ok(prediction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving prediction with ID {id}");
                return StatusCode(500, "An error occurred while retrieving prediction");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrediction(int id)
        {
            try
            {
                var result = await _predictionService.DeletePredictionAsync(id);
                
                if (!result)
                    return NotFound();
                    
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting prediction with ID {id}");
                return StatusCode(500, "An error occurred while deleting prediction");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditPrediction(int id, [FromBody] PredictionRequestDto predictionRequest)
        {
            try
            {
                var prediction = await _predictionService.EditPredictionAsync(id, predictionRequest);
                
                if (prediction == null)
                    return NotFound();
                    
                return Ok(prediction);
            }
            catch (DuplicatePredictionException ex)
            {
                _logger.LogWarning(ex, "Duplicate prediction attempt during edit");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error editing prediction with ID {id}");
                return StatusCode(500, "An error occurred while editing prediction");
            }
        }
    }
}
