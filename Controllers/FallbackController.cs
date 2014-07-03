/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Portal.Controllers
{
    public class FallbackController : Controller
    {
        [OutputCache(Duration=60*60,VaryByParam="*",Location=OutputCacheLocation.ServerAndClient)]
        public ActionResult Proxy(string url)
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.FallbackUrl)) return HttpNotFound();

            var client = new WebClient();
            client.Encoding = Encoding.UTF8;

            var result = client.DownloadString("Properties.Settings.Default.FallbackUrl" + url);

            return Content(result,client.ResponseHeaders["Content-Type"]);
        }
	}
}