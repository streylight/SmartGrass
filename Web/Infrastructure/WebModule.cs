using Ninject.Modules;
using Service.Interfaces;
using Service.Services;

namespace Web.Infrastructure {
    /// <summary>
    /// The WebModule class
    /// </summary>
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
            Bind<ISettingsService>().To<SettingsService>();
        }
    } // class
} // namespace