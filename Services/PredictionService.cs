using BrewerPredictorApi.Data;
using BrewerPredictorApi.Models.Dtos;
using BrewerPredictorApi.Models.Entities;
using BrewerPredictorApi.Services.Exceptions;
using BrewerPredictorApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrewerPredictorApi.Services
{
    public class PredictionService : IPredictionService
    {
        private readonly ApplicationDbContext _context;

        public PredictionService(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<bool> IsDuplicatePredictionAsync(string firstName, string lastName, int? excludeId = null)
        {
            // Normalize the input (trim whitespace and convert to lowercase)
            string normalizedFirstName = firstName?.Trim().ToLower();
            string normalizedLastName = lastName?.Trim().ToLower();

            // Query for existing predictions with the same name
            var query = _context.Predictions
                .Where(p => p.FirstName.Trim().ToLower() == normalizedFirstName && 
                           p.LastName.Trim().ToLower() == normalizedLastName);

            // Exclude the current prediction if we're editing
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }

            // Check if any matching predictions exist
            return await query.AnyAsync();
        }

        public async Task<PredictionDto> AddPredictionAsync(PredictionRequestDto predictionRequest)
        {
            try
            {
                // Check for duplicate prediction
                bool isDuplicate = await IsDuplicatePredictionAsync(predictionRequest.FirstName, predictionRequest.LastName);
                if (isDuplicate)
                {
                    throw new DuplicatePredictionException($"A prediction already exists for {predictionRequest.FirstName} {predictionRequest.LastName}");
                }

                var prediction = new Prediction
                {
                    FirstName = predictionRequest.FirstName,
                    LastName = predictionRequest.LastName ?? string.Empty,
                    Wins = predictionRequest.Wins,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                };
                
                _context.Predictions.Add(prediction);
                await _context.SaveChangesAsync();
                
                return new PredictionDto
                {
                    Id = prediction.Id,
                    FirstName = prediction.FirstName,
                    LastName = prediction.LastName,
                    Wins = prediction.Wins,
                    CreatedOn = prediction.CreatedOn,
                    UpdatedOn = prediction.UpdatedOn
                };
            }
            catch (DuplicatePredictionException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<PredictionDto>> GetPredictionsAsync(int? id = null, string firstName = null, string lastName = null)
        {
            try
            {
                IQueryable<Prediction> query = _context.Predictions;
                
                if (id.HasValue)
                    query = query.Where(p => p.Id == id.Value);
                    
                if (!string.IsNullOrEmpty(firstName))
                    query = query.Where(p => p.FirstName.Contains(firstName));
                    
                if (!string.IsNullOrEmpty(lastName))
                    query = query.Where(p => p.LastName.Contains(lastName));
                    
                var predictions = await query.ToListAsync();
                
                return predictions.Select(p => new PredictionDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Wins = p.Wins,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn
                }).ToList();
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeletePredictionAsync(int id)
        {
            try
            {
                var prediction = await _context.Predictions.FindAsync(id);
                
                if (prediction == null)
                    return false;
                    
                _context.Predictions.Remove(prediction);
                await _context.SaveChangesAsync();
                
                return true;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PredictionDto> EditPredictionAsync(int id, PredictionRequestDto predictionRequest)
        {
            try
            {
                var prediction = await _context.Predictions.FindAsync(id);
                
                if (prediction == null)
                    return null;
                
                // Check for duplicate prediction, excluding the current one
                bool isDuplicate = await IsDuplicatePredictionAsync(predictionRequest.FirstName, predictionRequest.LastName, id);
                if (isDuplicate)
                {
                    throw new DuplicatePredictionException($"A prediction already exists for {predictionRequest.FirstName} {predictionRequest.LastName}");
                }
                    
                prediction.FirstName = predictionRequest.FirstName;
                prediction.LastName = predictionRequest.LastName ?? string.Empty;
                prediction.Wins = predictionRequest.Wins;
                prediction.UpdatedOn = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                
                return new PredictionDto
                {
                    Id = prediction.Id,
                    FirstName = prediction.FirstName,
                    LastName = prediction.LastName,
                    Wins = prediction.Wins,
                    CreatedOn = prediction.CreatedOn,
                    UpdatedOn = prediction.UpdatedOn
                };
            }
            catch (DuplicatePredictionException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }
    }
} 