using System.Data.Entity.ModelConfiguration;
using Core.Domains;

namespace Map.Mappings {
    /// <summary>
    /// The mapping for the settings class
    /// </summary>
    public class SettingsMap : EntityTypeConfiguration<Settings> {
        public SettingsMap() {
            // Table
            ToTable( "Settings", "dbo" );

            // Primary key
            HasKey( u => u.Id );

            // Relationships

            //HasRequired(x => x.Unit)
            //    .WithOptional(x => x.Settings);
        }
    } // class
} // namespace
