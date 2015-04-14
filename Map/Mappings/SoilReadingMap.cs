using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the soil reading class
    /// </summary>
    public class SoilReadingMap : EntityTypeConfiguration<SoilReading> {
        public SoilReadingMap() {
            ToTable("SoilReading", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId);
        }
    }
}
