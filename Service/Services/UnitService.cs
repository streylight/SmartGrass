using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the unit class
    /// </summary>
    public class UnitService : IUnitService{

        private readonly IRepository<Unit> _unitRepository;

        public UnitService(){
            _unitRepository = new Repository<Unit>();    
        }

        public Unit GetUnitById(int id){
            return _unitRepository.GetById(id);
        }

        public IList<Unit> GetAllUnits(){
            return _unitRepository.Table.ToList();
        }

        public void Insert(Unit unit){
            try{
                if (unit.Id == 0)
                    _unitRepository.Insert(unit);
                else
                    _unitRepository.Update(unit);    

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var unit = _unitRepository.GetById(id);
                _unitRepository.Delete(unit);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public int ValidateProductKey(string productKey) {
            var unit = _unitRepository.Table.FirstOrDefault(x => x.ProductKey == productKey);
            return unit != null ? unit.Id : -1;
        }
    }
}
