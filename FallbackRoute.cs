/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal
{
    public class FallbackRoute: RouteBase
    {
        private string _controller;
        private string _action;
        public FallbackRoute(string controller, string action)
        {
            _controller = controller;
            _action = action;
        }


        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var value = new RouteData(this, new MvcRouteHandler());
            value.Values.Add("controller", _controller);
            value.Values.Add("action", _action);
            try
            {
                value.Values.Add("url", httpContext.Request.RawUrl);
            }
            catch (NotImplementedException)
            {
                value.Values.Add("url", httpContext.Request.AppRelativeCurrentExecutionFilePath.Replace("~",""));
            }
            return value;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}