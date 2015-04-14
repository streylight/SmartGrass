using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the temperature reading class
    /// </summary>
    public class TemperatureReadingMap : EntityTypeConfiguration<TemperatureReading> {
        public TemperatureReadingMap() {
            ToTable("TemperatureReading", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId);
        }
    }
}
