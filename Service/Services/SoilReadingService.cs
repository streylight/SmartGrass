using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the soil reading class
    /// </summary>
    public class SoilReadingService : ISoilReadingService{

        private readonly IRepository<SoilReading> _soilReadingRepository;

        public SoilReadingService(){
            _soilReadingRepository = new Repository<SoilReading>();    
        }

        public SoilReading GetSoilReadingById(int id){
            return _soilReadingRepository.GetById(id);
        }

        public IList<SoilReading> GetAllSoilReadings(){
            return _soilReadingRepository.Table.ToList();
        }

        public void Insert(List<SoilReading> soilReadings, int unitId){
            try{
                var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
                foreach (var soilReading in soilReadings) {
                    if (soilReading.Id == 0) {
                        //soilReading.DateTime = now;
                        soilReading.UnitId = unitId;
                        _soilReadingRepository.Insert(soilReading);
                    }
                }
            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var soilReading = _soilReadingRepository.GetById(id);
                _soilReadingRepository.Delete(soilReading);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }
}
