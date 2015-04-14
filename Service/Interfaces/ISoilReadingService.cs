using Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the soil reading service
    /// </summary>
    public interface ISoilReadingService {
        SoilReading GetSoilReadingById(int id);
        IList<SoilReading> GetAllSoilReadings();
        void Insert(List<SoilReading> soilReadings);
        void Delete(int id);
    }
}
