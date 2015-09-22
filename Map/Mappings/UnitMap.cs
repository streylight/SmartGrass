using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the unit class
    /// </summary>
    public class UnitMap : EntityTypeConfiguration<Unit> {
        public UnitMap() {
            // Table
            ToTable( "Unit", "dbo" );

            // Primary key
            HasKey( u => u.Id );

            // Relationships
            HasMany( x => x.RainEvents )
                .WithRequired()
                .HasForeignKey( x => x.UnitId );

            HasMany( x => x.IrrigationValves )
                .WithRequired()
                .HasForeignKey( x => x.UnitId );

            HasMany( x => x.SoilReadings )
                .WithRequired()
                .HasForeignKey( x => x.UnitId );

            HasMany( x => x.TemperatureReadings )
                .WithRequired()
                .HasForeignKey( x => x.UnitId );

            HasOptional( x => x.Settings )
                .WithMany()
                .HasForeignKey( x => x.SettingsId );
        }
    } // class
} // namespace
