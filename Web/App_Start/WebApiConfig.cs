using System.Web.Http;

namespace Web {
    /// <summary>
    /// The web API configuration class
    /// </summary>
    public static class WebApiConfig {
        /// <summary>
        /// Register the routes for the API calls
        /// </summary>
        /// <param name="config"></param>
        public static void Register( HttpConfiguration config ) {
			config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    } // class
} // namespace
