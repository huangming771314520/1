using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Newtonsoft.Json.Linq;
using Zephyr.Areas.Mms;

namespace Zephyr.Areas
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{Service}/{resource}.asmx/{*pathInfo}");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapPageRoute("Report", "report", "~/Content/page/report.aspx");

            routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "Zephyr.Controllers" }
             );
            //routes.IgnoreRoute("{Service}/{*x}", new { x = @".*\.asmx(/.*)?" });
 
            ModelBinders.Binders.Add(typeof(JObject), new JObjectModelBinder()); //for dynamic model binder
        }
    }
}