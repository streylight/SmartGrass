using System.Collections.Generic;
using System.Data.Entity;
using Core.Domains;

namespace Map.Repo {
    /// <summary>
    /// The IDbContext interface
    /// </summary>
    public interface IDbContext {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        int SaveChanges();
        IList<TEntity> ExecuteStoredProcedureList<TEntity>( string commandText, params object[] parameters )
            where TEntity : BaseEntity, new();
        IEnumerable<TElement> SqlQuery<TElement>( string sql, params object[] parameters );
        int ExecuteSqlCommand( string sql, int? timeout = null, params object[] parameters );
    } // class
} // namespace
