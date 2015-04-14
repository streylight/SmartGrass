using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the user class
    /// </summary>
    public class UserMap : EntityTypeConfiguration<User> {
        public UserMap() {
            ToTable("User", "dbo");
            HasKey(u => u.Id);

            // Relationships
            HasRequired(x => x.Unit)
                .WithMany()
                .HasForeignKey(x => x.UnitId);
        }
    }
}
