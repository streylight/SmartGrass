using System.Web.Mvc;

namespace Web {
    /// <summary>
    /// The filter configuration class
    /// </summary>
    public class FilterConfig {
        /// <summary>
        /// Registers all global filters
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters( GlobalFilterCollection filters ) {
            filters.Add( new AuthorizeAttribute() );
            filters.Add( new HandleErrorAttribute() );
        }
    } // class
} // namespace
