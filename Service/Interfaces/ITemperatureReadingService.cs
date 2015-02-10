using Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces {
    public interface ITemperatureReadingService {
        TemperatureReading GetTemperatureReadingById(int id);
        IList<TemperatureReading> GetAllTemperatureReadings();
        void Insert(TemperatureReading temperatureReading);
        void Delete(int id);
    }
}
