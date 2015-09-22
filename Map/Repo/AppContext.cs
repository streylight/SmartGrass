using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using Core.Domains;

namespace Map.Repo {
    /// <summary>
    /// The AppContext class
    /// </summary>
    public class AppContext : DbContext, IDbContext {
        
        /// <summary>
        /// Adds configuration types to model builder
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating( DbModelBuilder modelBuilder ) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            var registerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where( type => !String.IsNullOrEmpty( type.Namespace ) )
                .Where(
                    type =>
                        type.BaseType != null && type.BaseType.IsGenericType &&
                        type.BaseType.GetGenericTypeDefinition() == typeof( EntityTypeConfiguration<> ) );

            foreach ( var type in registerTypes ) {
                dynamic configInstance = Activator.CreateInstance( type );
                modelBuilder.Configurations.Add( configInstance );
            }
            base.OnModelCreating( modelBuilder );
        }

        /// <summary>
        /// Attaches an entity to a context
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual TEntity AttachEntityToContext<TEntity>( TEntity entity ) where TEntity : BaseEntity, new() {
            var existing = Set<TEntity>().Local.FirstOrDefault( x => x.Id == entity.Id );

            if ( existing != null ) {
                return existing;
            }
            Set<TEntity>().Attach(entity);
            return entity;
        }

        /// <summary>
        /// Detaches an entity from a context
        /// </summary>
        /// <param name="entity"></param>
        public void DetachEntityFromContext( object entity ) {
            var objContext = ( ( IObjectContextAdapter ) this ).ObjectContext;
            objContext.Detach( entity );
        }

        /// <summary>
        /// Creates a DB script
        /// </summary>
        /// <returns></returns>
        public string CreateDatabaseScript() {
            return ( ( IObjectContextAdapter ) this ).ObjectContext.CreateDatabaseScript();
        }

        /// <summary>
        /// Sets an entity to base entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// N/A
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IList<TEntity> ExecuteStoredProcedureList<TEntity>( string commandText, params object[] parameters ) where TEntity : BaseEntity, new() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generate a SQL query
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TElement> SqlQuery<TElement>( string sql, params object[] parameters ) {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        /// <summary>
        /// Execute a SQL command
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="timeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlCommand( string sql, int? timeout = null, params object[] parameters ) {
            int? prevTO = null;

            if ( timeout.HasValue ) {
                prevTO = ( ( IObjectContextAdapter ) this ).ObjectContext.CommandTimeout;
                ( ( IObjectContextAdapter ) this ).ObjectContext.CommandTimeout = timeout;
            }

            var result = this.Database.ExecuteSqlCommand( sql, parameters );

            if ( timeout.HasValue ) {
                ( ( IObjectContextAdapter ) this ).ObjectContext.CommandTimeout = prevTO;
            }

            return result;
        }
    } // class
} // namespace
