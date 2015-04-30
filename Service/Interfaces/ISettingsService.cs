using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the soil reading service
    /// </summary>
    public interface ISettingsService {
        Settings GetSettingsById(int id);
        IList<Settings> GetAllSettings();
        void Insert(Settings unitSettings);
        void Delete(int id);
    }
}
