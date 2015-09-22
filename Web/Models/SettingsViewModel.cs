using Core.Domains;

namespace Web.Models {
    /// <summary>
    /// The SettingsViewModel class
    /// </summary>
    public class SettingsViewModel : UserCredentials {
        public SettingsViewModel() { }
        public SettingsViewModel( User user ) {
            Username = user.Username;
            UserId = user.Id;
            UnitSettings = user.Unit.Settings ?? new Settings { RainLimit = 0, SoilMoistureLimit = 0 };
        }
        public Settings UnitSettings { get; set; }
        public int UserId { get; set; }
    } // class
} // namespace