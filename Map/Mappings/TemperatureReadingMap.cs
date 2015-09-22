using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the temperature reading class
    /// </summary>
    public class TemperatureReadingMap : EntityTypeConfiguration<TemperatureReading> {
        public TemperatureReadingMap() {
            // Table
            ToTable( "TemperatureReading", "dbo" );
            
            // Primary key
            HasKey( u => u.Id );

            // Relationships
            HasRequired( x => x.Unit )
                .WithMany()
                .HasForeignKey( x => x.UnitId );
        }
    } // class
} // namespace
