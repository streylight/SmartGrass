using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the watering event class
    /// </summary>
    public class WateringEventMap : EntityTypeConfiguration<WateringEvent> {
        public WateringEventMap() {
            ToTable("WateringEvent", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.IrrigationValve)
                .WithMany()
                .HasForeignKey(x => x.IrrigationValveId);
        }
    }
}
