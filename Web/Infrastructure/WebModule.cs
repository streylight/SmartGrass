using Ninject.Modules;
using Service;
using Service.Interfaces;

namespace Web.Infrastructure {
    public class WebModule : NinjectModule {
        /// <summary>
        /// Loads the module into the kernel
        /// </summary>
        public override void Load() {
            Bind<IIrrigationValveService>().To<IrrigationValveService>();
            Bind<IRainEventService>().To<RainEventService>();
            Bind<ISoilReadingService>().To<SoilReadingService>();
            Bind<IUnitService>().To<UnitService>();
            Bind<IUserService>().To<UserService>();
            Bind<IWateringEventService>().To<WateringEventService>();
            Bind<ITemperatureReadingService>().To<TemperatureReadingService>();
        }
    }
}