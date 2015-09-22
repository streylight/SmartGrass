using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the user class
    /// </summary>
    public class UserMap : EntityTypeConfiguration<User> {
        public UserMap() {
            // Table
            ToTable( "User", "dbo" );

            // Primary key
            HasKey(u => u.Id);

            // Relationships
            HasRequired( x => x.Unit )
                .WithMany()
                .HasForeignKey( x => x.UnitId );
        }
    } // class
} // namespace
