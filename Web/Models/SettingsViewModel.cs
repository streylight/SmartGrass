using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domains;

namespace Web.Models {
    public class SettingsViewModel : UserCredentials {
        public SettingsViewModel(User user) {
            Username = user.Username;
            UserId = user.Id;
            UnitSettings = user.Unit.Settings ?? new Settings { RainLimit = 0, SoilMoistureLimit = 0, UnitId = user.UnitId };
        }
        public Settings UnitSettings { get; set; }
        public int UserId { get; set; }
    }
}