using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the rain event class
    /// </summary>
    public class RainEventMap : EntityTypeConfiguration<RainEvent> {
        public RainEventMap() {
            // Table
            ToTable( "RainEvent", "dbo" );

            // Primary key
            HasKey( u => u.Id );

            // Relationships
            HasRequired( x => x.Unit )
                .WithMany()
                .HasForeignKey( x => x.UnitId );
        }
    } // class
} // namespace
