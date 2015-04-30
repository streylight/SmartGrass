using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the soil reading class
    /// </summary>
    public class SettingsService : ISettingsService{

        private readonly IRepository<Settings> _settingsRepository;

        public SettingsService(){
            _settingsRepository = new Repository<Settings>();    
        }

        public Settings GetSettingsById(int id){
            return _settingsRepository.GetById(id);
        }

        public IList<Settings> GetAllSettings(){
            return _settingsRepository.Table.ToList();
        }

        public void Insert(Settings unitSettings){
            try{
                if (unitSettings.Id == 0) {
                    _settingsRepository.Insert(unitSettings);
                } else {
                    _settingsRepository.Update(unitSettings);
                }
            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var settings = _settingsRepository.GetById(id);
                _settingsRepository.Delete(settings);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }
}
