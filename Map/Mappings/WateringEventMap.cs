using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the watering event class
    /// </summary>
    public class WateringEventMap : EntityTypeConfiguration<WateringEvent> {
        public WateringEventMap() {
            // Table
            ToTable( "WateringEvent", "dbo" );

            // Primary key
            HasKey( u => u.Id );

            // Relationships
            HasRequired( x => x.IrrigationValve )
                .WithMany()
                .HasForeignKey( x => x.IrrigationValveId );
        }
    } // class
} // namespace
