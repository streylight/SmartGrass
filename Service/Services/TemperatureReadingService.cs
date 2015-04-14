using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the temperature reading class
    /// </summary>
    public class TemperatureReadingService : ITemperatureReadingService{

        private readonly IRepository<TemperatureReading> _temperatureReadingRepository;

        public TemperatureReadingService(){
            _temperatureReadingRepository = new Repository<TemperatureReading>();    
        }

        public TemperatureReading GetTemperatureReadingById(int id){
            return _temperatureReadingRepository.GetById(id);
        }

        public IList<TemperatureReading> GetAllTemperatureReadings(){
            return _temperatureReadingRepository.Table.ToList();
        }

        public void Insert(TemperatureReading temperatureReading){
            try{
                if (temperatureReading.Id == 0)
                    _temperatureReadingRepository.Insert(temperatureReading);
                else
                    _temperatureReadingRepository.Update(temperatureReading);    

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var temperatureReading = _temperatureReadingRepository.GetById(id);
                _temperatureReadingRepository.Delete(temperatureReading);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }
}
