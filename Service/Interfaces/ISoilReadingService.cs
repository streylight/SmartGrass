using Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces {
    public interface ISoilReadingService {
        SoilReading GetSoilReadingById(int id);
        IList<SoilReading> GetAllSoilReadings();
        void Insert(SoilReading soilReading);
        void Delete(int id);
    }
}
