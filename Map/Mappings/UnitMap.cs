using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the unit class
    /// </summary>
    public class UnitMap : EntityTypeConfiguration<Unit> {
        public UnitMap() {
            ToTable("Unit", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasMany(x => x.RainEvents)
                .WithRequired()
                .HasForeignKey(x => x.UnitId);

            HasMany(x => x.IrrigationValves)
                .WithRequired()
                .HasForeignKey(x => x.UnitId);

            HasMany(x => x.SoilReadings)
                .WithRequired()
                .HasForeignKey(x => x.UnitId);

            HasMany(x => x.TemperatureReadings)
                .WithRequired()
                .HasForeignKey(x => x.UnitId);

            HasOptional(x => x.Settings)
                .WithMany()
                .HasForeignKey(x => x.SettingsId);

        }
    }
}
