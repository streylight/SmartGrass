using Core.Domains;
using Map.Repo;
using Ninject.Modules;
using Service.Interfaces;

namespace Service {
    public class DependancyModules : NinjectModule{

        public override void Load(){

            Bind<IDbContext>().To<AppContext>();
            Bind<IRepository<User>>().To<Repository<User>>();
            Bind<IRepository<IrrigationValve>>().To<Repository<IrrigationValve>>();
            Bind<IRepository<TemperatureReading>>().To<Repository<TemperatureReading>>();
            Bind<IRepository<SoilReading>>().To<Repository<SoilReading>>();
            Bind<IRepository<Zone>>().To<Repository<Zone>>();
            Bind<IRepository<Unit>>().To<Repository<Unit>>();
        }
    }
}
