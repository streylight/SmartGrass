using System.Web.Mvc;
using System.Web.Routing;

namespace Web {
    /// <summary>
    /// The route configuration class
    /// </summary>
    public class RouteConfig {
        /// <summary>
        /// Register all the routes
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes( RouteCollection routes ) {
            routes.IgnoreRoute( "{resource}.axd/{*pathInfo}" );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    } // class
} // namespace
