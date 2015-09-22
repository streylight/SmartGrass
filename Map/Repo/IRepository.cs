using System.Linq;
using Core.Domains;

namespace Map.Repo {
    /// <summary>
    /// The IRepository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract partial class IRepository<T> where T : BaseEntity {
        public abstract T GetById( object id );
        public abstract void Insert( T entity );
        public abstract void Update( T entity );
        public abstract void Delete( T entity );
        public abstract IQueryable<T> Table { get; }
    } // class
} // namespace