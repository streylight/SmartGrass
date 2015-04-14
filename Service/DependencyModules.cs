using Core.Domains;
using Map.Repo;
using Ninject.Modules;
using Service.Interfaces;

namespace Service {
    /// <summary>
    /// Interface to service bindings for dependency injection
    /// </summary>
    public class DependencyModules : NinjectModule {

        public override void Load() {
            Bind<IDbContext>().To<AppContext>();
            Bind<IRepository<User>>().To<Repository<User>>();
            Bind<IRepository<IrrigationValve>>().To<Repository<IrrigationValve>>();
            Bind<IRepository<TemperatureReading>>().To<Repository<TemperatureReading>>();
            Bind<IRepository<SoilReading>>().To<Repository<SoilReading>>();
            Bind<IRepository<RainEvent>>().To<Repository<RainEvent>>();
            Bind<IRepository<Unit>>().To<Repository<Unit>>();
            Bind<IRepository<WateringEvent>>().To<Repository<WateringEvent>>();
        }
    }
}
