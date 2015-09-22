
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using Core.Domains;

namespace Map.Repo {
    /// <summary>
    /// The Repository class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : BaseEntity {

        #region vars
        
        private readonly AppContext _appContext;
        private IDbSet<T> _entities;
        private IDbSet<T> Entities {
            get {
                return _entities ?? (_entities = _appContext.Set<T>());
            }
        }

        #endregion

        #region ctor
        /// <summary>
        /// Constructor for the repository class
        /// </summary>
        public Repository() {
            _appContext = new AppContext();
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns table entities
        /// </summary>
        public override IQueryable<T> Table {
            get {
                return Entities;
            }
        }
 
        /// <summary>
        /// Finds a single entity by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T GetById( object id ) {
            return Entities.Find( id );
        }

        /// <summary>
        /// Inserts a new entity into the db
        /// </summary>
        /// <param name="entity"></param>
        public override void Insert( T entity ) {
            try {
                if ( entity == null ) {
                    throw new ArgumentNullException( "entity" );
                }

                Entities.Add( entity );
                _appContext.SaveChanges();

            } catch ( DbEntityValidationException ex ) {
                var error = ex.EntityValidationErrors.Aggregate( "", 
                    ( current1, exception ) => exception.ValidationErrors.Aggregate( current1, 
                        ( current, innerException ) => current + ( string.Format( "Property: {0} Error: {1}", innerException.PropertyName, innerException.ErrorMessage ) + Environment.NewLine ) ) );

                throw new Exception( error, ex );
            }
        }

        /// <summary>
        /// Flags an entity as updated and updates the db
        /// </summary>
        /// <param name="entity"></param>
        public override void Update( T entity ) {
            try {
                if ( entity == null ) {
                    throw new ArgumentNullException( "entity" );
                }

                if ( _appContext.Entry( entity ).State == EntityState.Detached ) {
                    var prevAttached = Entities.Local.FirstOrDefault( x => x.Id == entity.Id );

                    if ( prevAttached != null ) {
                        _appContext.Entry( prevAttached ).CurrentValues.SetValues( entity );
                    } else {
                        _appContext.Entry( entity ).State = EntityState.Modified;
                    }
                }
                _appContext.SaveChanges();

            } catch ( DbEntityValidationException ex ) {
                var error = ex.EntityValidationErrors.Aggregate( "",
                    ( current1, exception ) => exception.ValidationErrors.Aggregate( current1,
                        ( current, innerException ) => current + ( string.Format( "Property: {0} Error: {1}", innerException.PropertyName, innerException.ErrorMessage ) + Environment.NewLine ) ) );

                throw new Exception( error, ex );
            }
        }

        /// <summary>
        /// Deletes an entity from the db
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete( T entity ) {
            try {
                if ( entity == null ) {
                    throw new ArgumentNullException( "entity" );
                }

                Entities.Remove( entity );
                _appContext.SaveChanges();

            } catch ( DbEntityValidationException ex ) {
                var error = ex.EntityValidationErrors.Aggregate( "",
                    ( current1, exception ) => exception.ValidationErrors.Aggregate( current1,
                        ( current, innerException ) => current + ( string.Format( "Property: {0} Error: {1}", innerException.PropertyName, innerException.ErrorMessage ) + Environment.NewLine ) ) );

                throw new Exception( error, ex );
            }
        }

        #endregion
    } // class
} // namespace
