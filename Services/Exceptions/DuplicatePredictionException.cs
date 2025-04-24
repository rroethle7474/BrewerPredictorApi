using System;

namespace BrewerPredictorApi.Services.Exceptions
{
    public class DuplicatePredictionException : Exception
    {
        public DuplicatePredictionException() : base() { }
        
        public DuplicatePredictionException(string message) : base(message) { }
        
        public DuplicatePredictionException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
} 