/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapSubdomainRoute("Home",
                "",
                new { controller = "Dashboard", action = "Public" },
                new[] { "Portal.Controllers" });

            routes.MapSubdomainRoute("FeaturedMenu",
                "child/Featured/{isMenu}",
                new { controller = "Dashboard", action = "Featured"},
                new[] { "Portal.Controllers" });

            routes.MapRoute("Logo",
                "child/Logo",
                new { controller = "Dashboard", action = "Logo" },
                new[] { "Portal.Controllers" });

            routes.MapSubdomainRoute("HomeTitle",
                "child/HomeTitle",
                new { controller = "Dashboard", action = "HomeTitle" },
                new[] { "Portal.Controllers" });         
            

            #region Thumbnails
            routes.MapSubdomainRoute("ThumbnailIndex",
                "thumbnail/{width}/{height}/{name}",
                new { controller = "Thumbnails", action = "Generate"},
                new[] { "Portal.Controllers" });
            #endregion

            #region News
            //routes.Add("Subdomain filter", new SubdomainRoute(
            //    "{domain}." + Properties.Settings.Default.BaseDomain,
            //    "news/{year}",
            //    new { controller = "News", action = "Index", year = 2014 },
            //    new { year = @"\d+" },
            //    new[] { "Portal.Controllers" })
            //); 

            routes.MapSubdomainRoute("NewsIndex",
                "news/{year}",
                new { controller = "News", action = "Index", year = 2014 },
                new { year = @"\d+" },
                new[] { "Portal.Controllers" });

            routes.MapSubdomainRoute("NewsRSS",
                "rss",
                new { controller = "News", action = "Rss" },
                new string[] { "Portal.Controllers" });

            routes.MapRoute("NewsItem",
                "news/{*slug}",
                new { controller = "News", action = "Item"},
                new string[] { "Portal.Controllers" });
                                   
            #endregion

            #region Timeline
            routes.MapRoute("Legacy Details Url",
                    "timeline/details/{slug}",
                    new { controller = "Timeline", action = "Details" },
                    new string[] { "Portal.Controllers" });

            routes.MapRoute("Legacy Partner Url",
                "timeline/partner/{slug}",
                new { controller = "Timeline", action = "Partner" },
                new string[] { "Portal.Controllers" });


            routes.MapSubdomainRoute("Timeline",
                    "timeline/{season}",
                    new { controller = "Timeline", action = "Index", season = 2014 },
                    new { season = @"\d+" },
                    new[] { "Portal.Controllers" });

            routes.MapRoute("OfficialEvent",
                    "timeline/{slug}/{action}/{series}/{id}",
                    new { controller = "Timeline", action = "Get", id=UrlParameter.Optional, series = UrlParameter.Optional},
                    new string[] { "Portal.Controllers" });



            routes.MapSubdomainRoute("Search",
                        "search/{action}",
                        new { controller = "Search" },
                        new string[] { "Portal.Controllers" });
            #endregion

            #region Documents

            routes.MapSubdomainRoute("Documents_root",
                "documents",
                new { controller = "Documents", action = "Index", season = 2014 },                
                new[] { "Portal.Controllers" });
            routes.MapSubdomainRoute("Documents",
                "documents/{season}",
                new { controller = "Documents", action = "Index", season = 2014 },
                new { season = @"\d+" },
                new[] { "Portal.Controllers" });

            routes.MapSubdomainRoute("FileAccessor",
                "file/get/{filename}",
                new { controller = "Documents", action = "Get" },
                new[] { "Portal.Controllers" });

            routes.MapSubdomainRoute("Documents_2",
                "file",
                new { controller = "Documents", action = "Index", season = 2014 },
                new[] { "Portal.Controllers" });

            routes.MapSubdomainRoute("Documents_3",
                "file/get",
                new { controller = "Documents", action = "Index", season = 2014 },
                new[] { "Portal.Controllers" });

            routes.MapSubdomainRoute("FileAccessor_new",
                "file/{filename}",
                new { controller = "Documents", action = "Get" },
                new[] { "Portal.Controllers" });
            #endregion

            #region Media

            routes.MapSubdomainRoute("Media Root",
                        "media",
                        new { controller = "Media", action = "Index" },
                        new string[] { "Portal.Controllers" });

            routes.MapSubdomainRoute("Gallery",
                        "media/{slug}",
                        new { controller = "Media", action="Gallery" },
                        new string[] { "Portal.Controllers" });

            #endregion

            routes.MapSubdomainRoute("account",
                "account/{action}/{id}",
                new { controller="account", action = "Index", id = UrlParameter.Optional });


            routes.Add(new FallbackRoute("Fallback", "Proxy"));

            //routes.MapRoute(
            //     "Fallback_legacy",
            //     "{*url}",
            //     new { controller = "Fallback", action = "Proxy" }
            // );


        }
    }

    public static class RouteConfigHelper
    {
        public static Route MapSubdomainRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            var route = new SubdomainRoute(
               "{domain}." + Properties.Settings.Default.BaseDomain,
               url,
               defaults);
            routes.Add(name, route);
            return route;
        }
        public static Route MapSubdomainRoute(this RouteCollection routes, string name, string url, object defaults,string[] namespaces)
        {
            var route = new SubdomainRoute(
               "{domain}." + Properties.Settings.Default.BaseDomain,
               url,
               defaults,
               namespaces);
            routes.Add(name, route);
            return route;
        }
        public static Route MapSubdomainRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            var route = new SubdomainRoute(
               "{domain}." + Properties.Settings.Default.BaseDomain,
               url,
               defaults,
               constraints,
               namespaces);
            routes.Add(name, route);
            return route;
        }
    }
}
