using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Map.Mappings {
    public class RainEventMap : EntityTypeConfiguration<RainEvent> {
        /// <summary>
        /// The mapping for the rain event
        /// </summary>
        public RainEventMap() {
            ToTable("RainEvent", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId);
        }
    }
}
