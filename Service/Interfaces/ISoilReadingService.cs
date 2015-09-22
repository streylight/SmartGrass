using Core.Domains;
using System.Collections.Generic;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the soil reading service
    /// </summary>
    public interface ISoilReadingService {
        SoilReading GetSoilReadingById( int id );
        IList<SoilReading> GetAllSoilReadings();
        void Insert( List<SoilReading> soilReadings, int unitId );
        void Delete( int id );
    } // class
} // namespace
