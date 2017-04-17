using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BlogClientNew
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");                       

            //Post display route
            routes.MapRoute(
                "Post",
                "Archive/{year}/{month}/{title}",
                new { controller="Blog",action="Post"}
            );

            //Tag link Routes..
            routes.MapRoute(
                "Tag",
                "Tag/{tag}",
                new{controller="Blog",action="Tag"}
            );

            //Categories links route...
            routes.MapRoute(
                "Category",
                "Category/{category}",
                new { controller="Blog", action="Category"}
            );

            //Login
            routes.MapRoute(
                "Login",
                "Login",
                new { controller = "Admin", action = "Login" }
            );

            //Logout Route
            routes.MapRoute(
                "Logout",
                "Logout",
                new { controller = "Admin", action = "Logout" }
            );

            //Manage Posts
            routes.MapRoute(
                "Manage",
                "Manage",
                new { controller="Manage", action="Manage"}
            );

            routes.MapRoute(
                "AdminAction",
                "Admin/{action}",
                new { controller="Admin",action="Login"}
            );

            //Default route of application..
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "Posts", id = UrlParameter.Optional }
            );
        }
    }
}