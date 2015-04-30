using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Map.Mappings {
    public class SettingsMap : EntityTypeConfiguration<Settings> {
        /// <summary>
        /// The mapping for the settings
        /// </summary>
        public SettingsMap() {
            ToTable("Settings", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Unit)
                .WithOptional(x => x.Settings);
        }
    }
}
