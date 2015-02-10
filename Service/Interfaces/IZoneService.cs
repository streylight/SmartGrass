using Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces {

    public interface IZoneService {
        Zone GetZoneById(int id);
        IList<Zone> GetAllZones();
        void Insert(Zone zone);
        void Delete(int id);
    }
}
