using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    public class UserMap : EntityTypeConfiguration<User> {
        public UserMap() {
            ToTable("User", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId);
        }
    }

    public class IrrigationValveMap : EntityTypeConfiguration<IrrigationValve> {
        public IrrigationValveMap() {
            ToTable("IrrigationValve", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Zone)
                .WithMany()
                .HasForeignKey(x => x.ZoneId);

            HasMany(x => x.SoilReadings)
                .WithRequired()
                .HasForeignKey(x => x.IrrigationValveId);
        }
    }

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

    public class ZoneMap : EntityTypeConfiguration<Zone> {
        public ZoneMap() {
            ToTable("Zone", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId);
        }
    }

    public class UnitMap : EntityTypeConfiguration<Unit> {
        public UnitMap() {
            ToTable("Unit", "dbo");
            HasKey(x => x.Id);

            // Relationships
            HasMany(x => x.Zones)
                .WithRequired()
                .HasForeignKey(x => x.UnitId);
        }
    }
}
