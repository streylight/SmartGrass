using Core.Domains;
using System.Collections.Generic;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the temperature reading service
    /// </summary>
    public interface ITemperatureReadingService {
        TemperatureReading GetTemperatureReadingById( int id );
        IList<TemperatureReading> GetAllTemperatureReadings();
        void Insert( TemperatureReading temperatureReading );
        void Delete( int id );
    } // class
} // namespace
