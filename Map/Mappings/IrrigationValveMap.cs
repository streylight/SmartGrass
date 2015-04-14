using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the irrigation valve class
    /// </summary>
    public class IrrigationValveMap : EntityTypeConfiguration<IrrigationValve> {
        public IrrigationValveMap() {
            ToTable("IrrigationValve", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId);

            HasMany(x => x.WateringEvents)
                .WithRequired()
                .HasForeignKey(x => x.IrrigationValveId);
        }
    }
}
