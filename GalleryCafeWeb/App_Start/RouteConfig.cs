using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GalleryCafeWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Custom route for SignUp
            routes.MapRoute(
                name: "SignUp",
                url: "SignUp",
                defaults: new { controller = "SignUp", action = "SignUp" }
            );

            routes.MapRoute(
    name: "Reservation",
    url: "Reservation/{action}/{id}",
    defaults: new { controller = "Reservation", action = "Create", id = UrlParameter.Optional }
);


            // Default route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
