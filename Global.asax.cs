/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Helpers;
using Portal.Helpers.Subdomain;
using Portal.Migrations;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SportDataContext, Configuration>());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ThemeViewEngine());

            AreaRegistration.RegisterAllAreas();            
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);

            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new SubDomainViewBagInitialisationFilter());
        }
    }
}
