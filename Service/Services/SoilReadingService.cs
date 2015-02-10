using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
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

        public void Insert(SoilReading soilReading){
            try{
                if (soilReading.Id == 0)
                    _soilReadingRepository.Insert(soilReading);
                else
                    _soilReadingRepository.Update(soilReading);    

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
