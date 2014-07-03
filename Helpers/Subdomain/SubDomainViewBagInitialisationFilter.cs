/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Helpers.Subdomain
{
    //Нужен для правильной настройки домена в вызовах masterpage
    public class SubDomainViewBagInitialisationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["domain"] != null)
                filterContext.Controller.ViewBag.Domain = filterContext.RouteData.Values["domain"];
        }
    }
}