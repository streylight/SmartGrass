using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the irrigation valve class
    /// </summary>
    public class IrrigationValveMap : EntityTypeConfiguration<IrrigationValve> {
        public IrrigationValveMap() {
            // Table
            ToTable( "IrrigationValve", "dbo" );

            // Primary key
            HasKey( u => u.Id );

            // Relationships
            HasRequired( x => x.Unit )
                .WithMany()
                .HasForeignKey( x => x.UnitId );

            HasMany( x => x.WateringEvents )
                .WithRequired()
                .HasForeignKey( x => x.IrrigationValveId );
        }
    } // class
} // namespace
