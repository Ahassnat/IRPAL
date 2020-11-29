using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCIntro
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home",
                    action = "Index",
                    id = UrlParameter.Optional },
                //لتوجيه المتصفح تلقائي علي الكنترولر هاد اذا ما وضحت في الباث اي كنترولر من اي منطقة فبيعتبر هاد تلقائي
                namespaces: new[] { "MVCIntro.Controllers" }
            );
        }
    }
}
