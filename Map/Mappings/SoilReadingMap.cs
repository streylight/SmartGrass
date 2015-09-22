using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the soil reading class
    /// </summary>
    public class SoilReadingMap : EntityTypeConfiguration<SoilReading> {
        public SoilReadingMap() {
            // Table
            ToTable( "SoilReading", "dbo" );

            // Primary key
            HasKey( u => u.Id );

            // Relationships
            HasRequired( x => x.Unit )
                .WithMany()
                .HasForeignKey( x => x.UnitId );
        }
    } // class
} // namespace
